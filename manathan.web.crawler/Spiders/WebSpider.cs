namespace manathan.web.crawler.Spiders
{
    using System;
    using System.Threading.Tasks;
    using Connection;
    using Documents;

    public delegate void WebDocumentLoaded(object sender, WebDocumentLoadedEventArgs e);

    public class WebDocumentLoadedEventArgs : EventArgs
    {
        public WebDocument Document { get; set; }

        public WebDocumentLoadedEventArgs(WebDocument document)
        {
            Document = document;
        }
    }

    internal class WebSpider
    {
        readonly IWebClient webClient;

        public WebSpider(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        public event WebDocumentLoaded WebDocumentLoaded;

        protected void OnWebDocumentLoaded(WebDocumentLoadedEventArgs args)
        {
            if (WebDocumentLoaded != null)
            {
                WebDocumentLoaded(this, args);
            }
        }

        protected void CrawlPage(Uri url)
        {
            var downloadedDocument = webClient.DownloadString(url);
            var webDocument = new WebDocument(new AgilityPackHtmlDocument()).Loaded(url, downloadedDocument);
            var urls = webDocument.AllUrls;
            OnWebDocumentLoaded(new WebDocumentLoadedEventArgs(webDocument));
            Parallel.ForEach(urls, (uri, state, index) =>
                {
                    if (state.IsExceptional || state.IsStopped)
                    {
                        return;
                    }

                    CrawlPage(uri);
                });
        }

        public void Crawl(Uri uri)
        {
            CrawlPage(uri);
        }
    }
}
