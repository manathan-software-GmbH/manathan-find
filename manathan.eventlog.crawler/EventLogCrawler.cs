namespace manathan.eventlog.crawler
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using find;
    using find.Configuration;
    using find.Crawler;

    public class EventLogCrawler : ICrawler
    {
        static IndexedPages _searchConfig;

        public EventLogCrawler()
        {
            _searchConfig = IndexedPages.GetConfigSettings();
        }

        public virtual string CrawlerName
        {
            get { return "EventLogCrawler"; }
        }

        public void Crawl()
        {
            _searchConfig.GetPagesForCrawler(CrawlerName).ForEach(CrawlEventLog);
        }

        void CrawlEventLog(Page page)
        {
            var url = page.Url;
            var log = "application";
            var machineName = ".";
            if (page.Options.HasKey("log"))
            {
                log = page.Options["log"].Value;
            }

            if (page.Options.HasKey("machine"))
            {
                machineName = page.Options["machine"].Value;
            }

            var eventLog = page.Options.HasKey("source") ? new EventLog(log, machineName, page.Options["source"].Value) : new EventLog(log, machineName);
            
            Parallel.ForEach(eventLog.Entries.Cast<EventLogEntry>(), entry => SearchEngine.AddDocument(new LogDocument(entry), page));
        }
    }
}
