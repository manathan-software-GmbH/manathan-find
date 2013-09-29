namespace manathan.find.Configuration
{
    #region

    using System;
    using System.Configuration;
    using System.Reflection;
    using Utils;

    #endregion

    public class Rule : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string RuleType
        {
            get { return (string) this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("settings", IsRequired = false)]
        [ConfigurationCollection(typeof(RuleSetting), AddItemName = "item")]
        public ConfigurationElementCollection<RuleSetting> Settings
        {
            get { return (ConfigurationElementCollection<RuleSetting>)this["settings"]; }
            set { this["settings"] = value; }
        }

        public T GetRule<T>() where T : class
        {
            var constructorInfo = GetRuleType().GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
            {
                return default(T);
            }

            var rule = (T)constructorInfo.Invoke(null);
            foreach (RuleSetting setting in Settings)
            {
                GetRuleType().GetProperty(setting.Key).SetValue(rule, setting.Value);
            }
            return rule;
        }

        public Type GetRuleType()
        {
            return Type.GetType(RuleType);
        }

        public Assembly GetAssembly()
        {
            var type = Type.GetType(RuleType);
            return (type != null) ? type.Assembly : null;
        }
    }

    public class RuleSetting : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string) this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (string) this["value"]; }
            set { this["value"] = value; }
        }
    }
}