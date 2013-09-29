namespace manathan.web.crawler.Documents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HtmlAgilityPack;

    public class AgilityPackHtmlDocument : IHtmlDocumentStrategy
    {
        readonly HtmlDocument document;

        public AgilityPackHtmlDocument()
        {
            document = new HtmlDocument();
        }

        public void LoadHtml(string htmlDocument)
        {
            document.LoadHtml(htmlDocument);
        }

        public string GetInnerText(string element)
        {
            var nodes = document.DocumentNode.Descendants(element);
            var fullContent = new StringBuilder();
            foreach (var node in nodes)
            {
                fullContent.AppendLine(node.InnerText);
            }
            return fullContent.ToString();
        }

        public string GetAttributePair(string element, string key, string value)
        {
            var nodes = document.DocumentNode.Descendants(element);
            var fullContent = new StringBuilder();
            foreach (var node in nodes)
            {
                fullContent.AppendFormat("{0}={1}", key, node.Attributes[key].Value);
                fullContent.AppendLine();
            }
            return fullContent.ToString();
        }

        public string AllWords { get { return document.DocumentNode.InnerText; }}

        public List<string> GetAttributeList(string element, string attribute)
        {
            var nodes = document.DocumentNode.Descendants(element);
            return (from node in nodes 
                               select node.Attributes[attribute] 
                               into htmlAttribute 
                               where htmlAttribute != null 
                               select htmlAttribute.Value).ToList();
        }
    }
}