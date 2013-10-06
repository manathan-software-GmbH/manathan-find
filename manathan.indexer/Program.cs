[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace manathan.indexer
{
    #region

    using System;
    using find;

    #endregion

    internal class Program
    {
        static void Main()
        {
            try
            {
                SearchEngine.Initialize(true);
                SubscribeLoggers();
                SearchEngine.CreateIndex();
            }
            catch (Exception e0)
            {
                WriteError(e0, "manathan.indexer stopped unexpected");
            }
        }

        static void SubscribeLoggers()
        {
            EngineStatus.CrawlPageBegin += (sender, args) => WriteInfo("Started indexing of page {0}", args.Page.Url);
            EngineStatus.CrawlPageComplete += (sender, args) => WriteInfo("Completed indexing of page {0}", args.Page.Url);

            EngineStatus.CrawlDocumentBegin += (sender, args) => WriteInfo("Started indexing of document {0} on page {1}", args.Document.Title, args.Page.Url);
            EngineStatus.CrawlDocumentBegin += (sender, args) => WriteInfo("Completed indexing of document {0} on page {1}", args.Document.Title, args.Page.Url);

            SearchEngine.CrawlerBegin +=
                (sender, args) => WriteInfo("Crawler {0} started working...", args.CurrentCrawler.GetType().Name);
            SearchEngine.CrawlerFailed +=
                (sender, args) =>
                WriteError(args.Error, "Crawler {0} failed unexpected: {1}", args.CurrentCrawler.GetType().Name,
                           args.Error.Message);
            SearchEngine.CrawlerComplete +=
                (sender, args) => WriteInfo("Crawler {0} completed the work successfully", args.CurrentCrawler.GetType().Name);
        }

        static void WriteInfo(string message, params object[] paras)
        {
            log4net.LogManager.GetLogger("manathan.indexer").Info(string.Format(message, paras));
        }

        static void WriteError(Exception error, string message, params object[] paras)
        {
            log4net.LogManager.GetLogger("manathan.indexer").Error(string.Format(message, paras), error);
        }
    }
}