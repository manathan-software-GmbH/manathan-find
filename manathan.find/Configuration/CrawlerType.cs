namespace manathan.find.Configuration
{
    #region

    using System.Configuration;

    #endregion

    public class CrawlerType : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "DefaultCrawler", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "Search.Crawler.DefaultCrawler, Search", IsRequired = false,
            IsKey = false)]
        public string Type
        {
            get { return (string) this["type"]; }
            set { this["type"] = value; }
        }
    }
}