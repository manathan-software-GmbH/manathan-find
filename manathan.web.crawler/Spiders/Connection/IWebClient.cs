namespace manathan.web.crawler.Spiders.Connection
{
    using System;

    internal interface IWebClient
    {
        string DownloadString(Uri url);
    }

    internal class WebClient : IWebClient
    {
        public string DownloadString(Uri url)
        {
            using (var webClient = new System.Net.WebClient())
            {
                return webClient.DownloadString(url);
            }
        }
    }
}