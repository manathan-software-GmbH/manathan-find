namespace manathan.find.Configuration
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Lucene.Net.Analysis;
    using Lucene.Net.Store;
    using Utils;
    using find.Crawler;

    #endregion

    public class Crawler : ConfigurationElement
    {
        [ConfigurationProperty("crawlers", IsRequired = false)]
        [ConfigurationCollection(typeof (CrawlerType), AddItemName = "item")]
        public ConfigurationElementCollection<CrawlerType> Crawlers
        {
            get { return (ConfigurationElementCollection<CrawlerType>) this["crawlers"]; }
        }


        ICrawler CrawlerFactory(string crawlerType, Directory directory, Analyzer analyzer)
        {
            var type = Type.GetType(crawlerType);
            if (type == null)
                return null;

            var constructor = type.GetConstructor(new[] {typeof (Directory), typeof (Analyzer)});
            if (constructor != null)
            {
                return (ICrawler) constructor.Invoke(new object[] {directory, analyzer});
            } 
            constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                return (ICrawler)constructor.Invoke(null);
            }
            return null;
        }

        public IEnumerable<ICrawler> CrawlersFactory(Directory directory, Analyzer analyzer)
        {
            var config = IndexedPages.GetConfigSettings();
            var crawlers = config.Pages.ToList()
                                 .Select(_ => Crawlers.ToList().First(crawler => crawler.Name == _.Crawler).Type)
                                 .Distinct()
                                 .ToList()
                                 .Select(_ => CrawlerFactory(_, directory, analyzer));
            return crawlers;
        }
    }
}