namespace manathan.web.crawler
{
    using System;
    using System.Collections.Generic;
    using Documents;
    using find.Crawler;

    public sealed class WebDocument : BaseDocument
    {
        readonly IHtmlDocumentStrategy htmlDocument;
        List<Uri> allUrls;

        public WebDocument(IHtmlDocumentStrategy htmlDocument)
        {
            this.htmlDocument = htmlDocument;
        }

        public WebDocument Loaded(Uri url, string downloadedDocument)
        {
            htmlDocument.LoadHtml(downloadedDocument);
            Title = htmlDocument.GetInnerText("title");
            Content = htmlDocument.GetInnerText("body");
            List<string> hrefs = htmlDocument.GetAttributeList("a", "href");
            MetaContext = htmlDocument.GetAttributePair("meta", "name", "content");
            Uri = url;
            return this;
        }

        public List<Uri> AllUrls
        {
            get { return allUrls; }
        }

        public override string ToSearchable()
        {
            return htmlDocument.AllWords;
        }
    }
}