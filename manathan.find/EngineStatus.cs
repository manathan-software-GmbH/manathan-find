namespace manathan.find
{
    #region

    using Configuration;
    using Crawler;
    using Events;

    #endregion

    public class EngineStatus
    {
        public static event CrawlPageBegin CrawlPageBegin;

        public static event CrawlPageComplete CrawlPageComplete;

        public static event CrawlDocumentBegin CrawlDocumentBegin;

        public static event CrawlDocumentComplete CrawlDocumentComplete;

        static void OnCrawlDocumentBegin(CrawlDocumentEventArgs args)
        {
            var handler = CrawlDocumentBegin;
            if (handler != null) handler(null, args);
        }

        static void OnCrawlDocumentComplete(CrawlDocumentEventArgs args)
        {
            var handler = CrawlDocumentComplete;
            if (handler != null) handler(null, args);
        }

        static void OnCrawlPageComplete(CrawlPageEventArgs args)
        {
            var handler = CrawlPageComplete;
            if (handler != null) handler(null, args);
        }

        static void OnCrawlPageBegin(CrawlPageEventArgs args)
        {
            var handler = CrawlPageBegin;
            if (handler != null) handler(null, args);
        }

        public static void BeginIndexPage(Page page)
        {
            OnCrawlPageBegin(new CrawlPageEventArgs(page));
        }

        public static void IndexPageComplete(Page page)
        {
            OnCrawlPageComplete(new CrawlPageEventArgs(page));
        }

        public static void BeginIndexDocument(Page page, BaseDocument document)
        {
            OnCrawlDocumentBegin(new CrawlDocumentEventArgs(document, page));
        }

        public static void IndexDocumentComplete(Page page, BaseDocument document)
        {
            OnCrawlDocumentComplete(new CrawlDocumentEventArgs(document, page));
        }
    }
}