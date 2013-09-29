namespace manathan.find.Configuration.Utils
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    #endregion

    /// <summary>
    /// The ConfigurationElementCollection&lt;t&gt; provides a simple generic implementation of ConfigurationElementCollection.  
    /// </summary>
    /// <remarks>
    /// You have to either use at least one ConfigurationPropertyAttribute with IsKey set to true
    /// or you have to override the ToString() member to get a unique key for each collection item
    /// </remarks>
    /// <example>
    /// <code>
    /// [ConfigurationProperty("myObjects", IsRequired = true)]
    /// [ConfigurationCollection(typeof(MyObject), AddItemName = "myObject")]
    /// public ConfigurationElementCollection&lt;MyObject&gt; MyObjects
    /// {
    ///     get { return (ConfigurationElementCollection&lt;MyObject&gt;)this["myObjects"]; }
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    [ConfigurationCollection(typeof (ConfigurationElement))]
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection
        where T : ConfigurationElement, new()
    {
        bool _readonly;

        public T this[int idx]
        {
            get { return (T) BaseGet(idx); }
        }

        public T this[object key]
        {
            get { return (T) BaseGet(key); }
        }

        public override bool IsReadOnly()
        {
            return _readonly;
        }

        public void MakeReadOnly()
        {
            _readonly = true;
        }

        public void MakeWriteable()
        {
            _readonly = false;
        }

        public void RemoveElement(T element)
        {
            int index = BaseGetAllKeys().TakeWhile(key => key.ToString() != GetElementKey(element).ToString()).Count();
            if (index >= base.Count)
                return;
            base.BaseRemoveAt(index);
        }

        public void AddElement(T element)
        {
            base.BaseAdd(element);
        }

        public void AddElements(ConfigurationElementCollection<T> elements)
        {
            foreach (T element in elements)
            {
                AddElement(element);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        public bool HasKey(object key)
        {
            return BaseGetAllKeys().Any(k => k.ToString() == key.ToString());
        }

        string CheckAttributes(ConfigurationElement element, PropertyInfo property,
                               IEnumerable<ConfigurationPropertyAttribute> attributes)
        {
            return attributes.Any(attribute => attribute.IsKey)
                       ? (property.GetValue(element, null).ToString())
                       : String.Empty;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var key = new StringBuilder();
            foreach (var property in element.GetType().GetProperties())
            {
                var attributes =
                    (ConfigurationPropertyAttribute[])
                    property.GetCustomAttributes(typeof (ConfigurationPropertyAttribute), true);
                if (attributes.Length < 1)
                    continue;
                key.Append(CheckAttributes(element, property, attributes));
            }
            return ((String.IsNullOrEmpty(key.ToString())) ? ((element)).ToString() : key.ToString());
        }

        public T[] ToArray()
        {
            return BaseGetAllKeys().Select(k => this[k]).ToArray();
        }

        public List<T> ToList()
        {
            return BaseGetAllKeys().Select(k => this[k]).ToList();
        }
    }
}