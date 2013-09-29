namespace manathan.file.crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using find.Crawler;

    public class LineDocument : BaseDocument
    {
        readonly string line;
        readonly Dictionary<string,string> columns = new Dictionary<string, string>();

        public LineDocument(string url, string line)
        {
            Uri = new Uri(url);
            this.line = line;
        }

        public string[] Headers { get { return columns.Keys.ToArray(); } }

        public override string ToSearchable()
        {
            return line;
        }

        public void WithSameHeadersAs(LineDocument document)
        {
            WithHeaders(document.Headers);
        }

        public void WithHeaders(string[] headerFields)
        {
            headerFields.ToList().ForEach(_ => columns.Add(_, string.Empty));
        }

        public void AddField(string header, string field)
        {
            columns[header] = field;
        }

        public string TryGet(string column)
        {
            return columns.ContainsKey(column) ? columns[column] : null;
        }
    }
}