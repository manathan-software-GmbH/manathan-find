namespace manathan.find.Crawler
{
    #region

    using Configuration;
    using Lucene.Net.Documents;

    #endregion

    public static class DocumentFactory<T> where T : BaseDocument
    {
        public static Document Create(Page page, T downloadedDocument)
        {
            var document = new Document();

            var content = !string.IsNullOrEmpty(downloadedDocument.Content) ? downloadedDocument.Content : downloadedDocument.ToSearchable();
            
            var bodyField = new Field("Content", content, Field.Store.YES, Field.Index.TOKENIZED);
            document.Add(bodyField);

            var dateField = new Field("Date", downloadedDocument.Date.ToString("yyyyMMdd"), Field.Store.YES,
                                      Field.Index.TOKENIZED);
            document.Add(dateField);

            var titleField = new Field("Title", downloadedDocument.Title, Field.Store.YES, Field.Index.TOKENIZED);
            document.Add(titleField);

            if (!string.IsNullOrEmpty(downloadedDocument.MetaContext))
            {
                var metaDataField = new Field("Meta", downloadedDocument.MetaContext, Field.Store.YES,
                                              Field.Index.TOKENIZED);
                document.Add(metaDataField);
            }

            var urlField = new Field("Url", downloadedDocument.Uri.OriginalString, Field.Store.YES,
                                     Field.Index.TOKENIZED);
            document.Add(urlField);
            return document;
        }
    }
}