namespace manathan.find.Configuration
{
    #region

    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Configuration;
    using Utils;

    #endregion

    public class IndexedPages : ConfigurationSection
    {
        const string Section = "search/index";

        [ConfigurationProperty("workerstorage", DefaultValue = "", IsRequired = false)]
        public string IndexWorkerStore
        {
            get { return (string) this["workerstorage"]; }
            set { this["workerstorage"] = value; }
        }

        [ConfigurationProperty("releasestorage", DefaultValue = "", IsRequired = true)]
        public string IndexReleaseStore
        {
            get { return (string) this["releasestorage"]; }
            set { this["releasestorage"] = value; }
        }

        [ConfigurationProperty("pages", IsRequired = false)]
        [ConfigurationCollection(typeof (Page), AddItemName = "page")]
        public ConfigurationElementCollection<Page> Pages
        {
            get { return (ConfigurationElementCollection<Page>) this["pages"]; }
            set { this["pages"] = value; }
        }

        [ConfigurationProperty("crawler", IsRequired = false)]
        public Crawler Crawler
        {
            get { return (Crawler) this["crawler"]; }
            set { this["crawler"] = value; }
        }

        public List<Page> GetPagesForCrawler(string crawlerName)
        {
            return Pages.ToList().Where(_ => _.Crawler == crawlerName).ToList();
        }

        public static IndexedPages GetConfigSettings()
        {
            return ((IndexedPages) WebConfigurationManager.GetSection(Section));
        }
    }
}