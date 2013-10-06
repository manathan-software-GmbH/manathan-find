namespace manathan.find.Events
{
    using Configuration;
    using Crawler;

    public class CrawlDocumentEventArgs : CrawlPageEventArgs
    {
        public BaseDocument Document { get; set; }

        public CrawlDocumentEventArgs(BaseDocument document, Page page) : base(page)
        {
            Document = document;
        }
    }
}