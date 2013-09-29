namespace manathan.find.Configuration
{
    #region

    using System.Configuration;

    #endregion

    public class Option : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string) this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Value
        {
            get { return (string) this["value"]; }
            set { this["value"] = value; }
        }
    }
}