namespace manathan.file.crawler
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Rules;
    using find;
    using find.Configuration;
    using find.Crawler;

    public class FileCrawler : ICrawler
    {
        static IndexedPages _searchConfig;

        public FileCrawler()
        {
            _searchConfig = IndexedPages.GetConfigSettings();
        }

        public virtual string CrawlerName
        {
            get { return "FileCrawler"; }
        }

        public void Crawl()
        {
            _searchConfig.GetPagesForCrawler(CrawlerName).ForEach(CrawlDirectory);
        }

        void CrawlDirectory(Page page)
        {
            var files = Directory.GetFiles(page.Url);
            var scopedPage = page;
            Parallel.ForEach(files, (file, state, fileIndex) =>
                {
                    try
                    {
                        var lines = File.ReadAllLines(file).Select(line => new LineDocument(page.Url, line)).ToList();
                        var firstLine = lines.FirstOrDefault();
                        if (firstLine == null) return;

                        // first line has to be handled seperatly to ensure header
                        page.GetAllRulesFor<LineRule>().ForEach(_ => _.ApplyTo(firstLine, 0));
                        SearchEngine.AddDocument(firstLine, scopedPage);

                        Parallel.ForEach(lines.GetRange(1, lines.Count - 1), (line, linestate, lineIndex) =>
                        {
                            try
                            {
                                line.WithSameHeadersAs(firstLine);
                                page.GetAllRulesFor<LineRule>().ForEach(_ => _.ApplyTo(line, lineIndex + 1));
                                SearchEngine.AddDocument(line, scopedPage);
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                    catch (Exception)
                    {

                    }
                });
        }
    }
}
