namespace manathan.find.Events
{
    using System;
    using Crawler;

    public class CrawlerFailedEventArgs : CrawlerEventArgs
    {
        public Exception Error { get; set; }

        public CrawlerFailedEventArgs(ICrawler currentCrawler, Exception error) : base(currentCrawler)
        {
            Error = error;
        }
    }
}