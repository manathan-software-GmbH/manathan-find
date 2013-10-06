namespace manathan.find.Events
{
    using System;
    using Crawler;

    public class CrawlerEventArgs : EventArgs
    {
        public ICrawler CurrentCrawler { get; set; }

        public CrawlerEventArgs(ICrawler currentCrawler)
        {
            CurrentCrawler = currentCrawler;
        }
    }
}