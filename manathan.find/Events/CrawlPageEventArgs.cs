namespace manathan.find.Events
{
    using System;
    using Configuration;

    public class CrawlPageEventArgs : EventArgs
    {
        public Page Page { get; set; }

        public CrawlPageEventArgs(Page page)
        {
            Page = page;
        }
    }
}