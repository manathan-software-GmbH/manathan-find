namespace manathan.find
{
    #region

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Configuration;
    using Crawler;
    using Events;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Models;
    using Directory = Lucene.Net.Store.Directory;
    using Hit = Models.Hit;

    #endregion

    public class SearchEngine
    {
        static FileInfo _path;
        static IndexedPages _searchConfig;
        static IndexWriter _indexWriter;

        static Directory Directory { get; set; }
        static Analyzer Analyzer { get; set; }

        public static event CrawlerBegin CrawlerBegin;

        public static event CrawlerComplete CrawlerComplete;

        public static event CrawlerFailed CrawlerFailed;

        static void OnCrawlerFailed(CrawlerFailedEventArgs args)
        {
            var handler = CrawlerFailed;
            if (handler != null) handler(null, args);
        }

        static void OnCrawlerBegin(CrawlerEventArgs args)
        {
            var handler = CrawlerBegin;
            if (handler != null) handler(null, args);
        }

        static void OnCrawlerComplete(CrawlerEventArgs args)
        {
            var handler = CrawlerComplete;
            if (handler != null) handler(null, args);
        }


        public static void Initialize(bool isIndexer = false)
        {
            _searchConfig = IndexedPages.GetConfigSettings();
            _path = new FileInfo(Path.Combine((isIndexer)
                                                 ? _searchConfig.IndexWorkerStore
                                                 : _searchConfig.IndexReleaseStore, "indexes"));
            var directoryExists = _path.Exists;
            var createDirectory = !directoryExists && isIndexer;
            Directory = FSDirectory.GetDirectory(_path, createDirectory);
            Analyzer = new StandardAnalyzer();
            if (isIndexer)
            {
                var indexExists = IndexReader.IndexExists(Directory);
                var createIndex = !indexExists;
                _indexWriter = new IndexWriter(Directory, Analyzer, createIndex);
                _indexWriter.Close();
            }
            Directory.Close();
        }

        public static void CreateIndex()
        {
            Directory = FSDirectory.GetDirectory(_path, !_path.Exists);
            Analyzer = new StandardAnalyzer();
            var indexExists = IndexReader.IndexExists(Directory);
            var createIndex = !indexExists;
            _indexWriter = new IndexWriter(Directory, Analyzer, createIndex);
            _searchConfig.Crawler.CrawlersFactory(Directory, Analyzer).ToList()
                        .ForEach(_ =>
                            {
                                try
                                {
                                    OnCrawlerBegin(new CrawlerEventArgs(_));
                                    _.Crawl();
                                    OnCrawlerComplete(new CrawlerEventArgs(_));
                                }
                                catch (Exception error)
                                {
                                    OnCrawlerFailed(new CrawlerFailedEventArgs(_, error));
                                }
                            });
            Release();
            _indexWriter.Close();
            Directory.Close();
        }

        static void Release()
        {
            var releasePath = Path.Combine(_searchConfig.IndexReleaseStore, "indexes");
            if (!System.IO.Directory.Exists(releasePath))
            {
                System.IO.Directory.CreateDirectory(releasePath);
            }

            System.IO.Directory.GetFiles(Path.Combine(_searchConfig.IndexWorkerStore, "indexes")).ToList()
                  .ForEach(file => File.Copy(file, Path.Combine(releasePath, Path.GetFileName(file)), true));
        }

        public static void AddDocument<T>(T downloadedDocument, Page page) where T : BaseDocument
        {
            EngineStatus.BeginIndexDocument(page, downloadedDocument);
            Debug.WriteLine("Adding a {0} downloaded from {1}", downloadedDocument.GetType(), downloadedDocument.Uri);

            var document = DocumentFactory<T>.Create(page, downloadedDocument);
            _indexWriter.AddDocument(document);
            EngineStatus.IndexDocumentComplete(page, downloadedDocument);
        }
        
        public HitCollection Search(string searchFor)
        {
            return Search(searchFor, string.Empty);
        }

        public HitCollection Search(string searchFor, string orderBy)
        {
            return Search(new[] {searchFor, searchFor, searchFor, searchFor}, new[] {"Content", "Title", "Url", "Date"},
                          orderBy);
        }

        public HitCollection Search(string[] searchFor, string[] searchWhere, string orderBy)
        {
            var indexSearcher = new IndexSearcher(Directory);
            Query query = MultiFieldQueryParser.Parse(searchFor, searchWhere, Analyzer);
            HitCollection hits;
            if (orderBy == "date")
            {
                var sort = new Sort(new SortField("Date", SortField.INT, true));
                hits = GetHits(indexSearcher.Search(query, sort));
            }
            else
            {
                hits = GetHits(indexSearcher.Search(query));
            }

            indexSearcher.Close();
            Directory.Close();
            return hits;
        }

        HitCollection GetHits(Hits hits)
        {
            var hitCollection = new HitCollection();

            for (int i = 0; i < hits.Length(); i++)
            {
                var content = hits.Doc(i).Get("Content").Replace(Environment.NewLine, " ");
                hitCollection.Add(new Hit
                    {
                        Url = new Uri(hits.Doc(i).Get("Url")),
                        Title = hits.Doc(i).Get("Title"),
                        Content = content.Substring(0, (content.Length > 500) ? 500 : content.Length)
                    });
            }
            return hitCollection;
        }
    }
}