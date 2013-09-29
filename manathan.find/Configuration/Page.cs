namespace manathan.find.Configuration
{
    #region

    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Utils;

    #endregion

    public class Page : ConfigurationElement
    {
        [ConfigurationProperty("options", IsRequired = false)]
        [ConfigurationCollection(typeof (Option), AddItemName = "item")]
        public ConfigurationElementCollection<Option> Options
        {
            get { return (ConfigurationElementCollection<Option>) this["options"]; }
        }

        [ConfigurationProperty("excluded", IsRequired = false)]
        [ConfigurationCollection(typeof (Exclusion), AddItemName = "item")]
        public ConfigurationElementCollection<Exclusion> Exclusions
        {
            get { return (ConfigurationElementCollection<Exclusion>) this["excluded"]; }
        }

        [ConfigurationProperty("matchByTitle", DefaultValue = false)]
        public bool MatchByTitle
        {
            get { return (bool) this["matchByTitle"]; }
            set { this["matchByTitle"] = value; }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Url
        {
            get { return (string) this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("rules", IsRequired = false)]
        [ConfigurationCollection(typeof (Rule), AddItemName = "item")]
        public ConfigurationElementCollection<Rule> Rules
        {
            get { return (ConfigurationElementCollection<Rule>) this["rules"]; }
            set { this["rules"] = value; }
        }

        [ConfigurationProperty("crawler", DefaultValue = "DefaultCrawler", IsRequired = false)]
        public string Crawler
        {
            get { return (string) this["crawler"]; }
            set { this["crawler"] = value; }
        }

        public List<T> GetAllRulesFor<T>() where T : class
        {
            return Rules.Cast<Rule>().Select(rule => rule.GetRule<T>()).Where(lineRule => lineRule != null).ToList();
        }
    }
}