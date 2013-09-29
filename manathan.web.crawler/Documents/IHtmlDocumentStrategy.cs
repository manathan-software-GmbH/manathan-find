namespace manathan.web.crawler.Documents
{
    using System.Collections.Generic;

    public interface IHtmlDocumentStrategy
    {
        void LoadHtml(string htmlDocument);

        string GetInnerText(string element);

        string GetAttributePair(string element, string key, string value);
        string AllWords { get; }
        List<string> GetAttributeList(string element, string attribute);
    }
}