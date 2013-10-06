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
                SearchEngine.CrawlerBegin += (sender, args) => WriteInfo("Crawler {0} started working...", args.CurrentCrawler.GetType().Name);
                SearchEngine.CrawlerFailed += (sender, args) => WriteError(args.Error, "Crawler {0} failed unexpected: {1}", args.CurrentCrawler.GetType().Name, args.Error.Message);
                SearchEngine.CrawlerComplete += (sender, args) => WriteInfo("Crawler {0} completed the work successfully", args.CurrentCrawler.GetType().Name);
                SearchEngine.CreateIndex();
            }
            catch (Exception e0)
            {
                WriteError(e0, "manathan.indexer stopped unexpected");
            }
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