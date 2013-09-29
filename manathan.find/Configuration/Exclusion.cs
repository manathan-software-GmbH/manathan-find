namespace manathan.find.Configuration
{
    #region

    using System.Configuration;

    #endregion

    public class Exclusion : ConfigurationElement
    {
        [ConfigurationProperty("uriContains", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string UrlContains
        {
            get { return (string) this["uriContains"]; }
            set { this["uriContains"] = value; }
        }
    }
}