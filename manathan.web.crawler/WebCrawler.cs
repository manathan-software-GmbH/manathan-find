namespace manathan.web.crawler
{
    #region

    using System;
    using Spiders;
    using Spiders.Connection;
    using find;
    using find.Configuration;
    using find.Crawler;

    #endregion

    public class WebCrawler : ICrawler
    {
        static IndexedPages _searchConfig;

        public WebCrawler()
        {
            _searchConfig = IndexedPages.GetConfigSettings();
        }

        public virtual string CrawlerName
        {
            get { return "DefaultCrawler"; }
        }

        public void Crawl()
        {
            _searchConfig.GetPagesForCrawler(CrawlerName).ForEach(QueryPage);
        }

        protected virtual void QueryPage(Page page)
        {
            try
            {
                var spider = new WebSpider(new WebClient());

                var uri = new Uri(page.Url);
                EngineStatus.BeginIndexPage(page);

                // store this to have the page available when the 
                //  inner closure is executed
                var closurePage = page;
                spider.WebDocumentLoaded += (source, args) => SearchEngine.AddDocument(args.Document, closurePage);
                spider.Crawl(uri);

                EngineStatus.IndexPageComplete(page);
            }
            catch (Exception e0)
            {
                Console.WriteLine(e0);
            }
        }
    }
}