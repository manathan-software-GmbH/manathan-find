namespace manathan.file.crawler.Rules
{
    using System;
    using System.Linq;
    using System.Text;

    public class SeperatedLineRule : LineRule
    {
        public string Seperator { get; set; }

        public string Headers { get; set; }

        public string HasHeader { get; set; }

        public string FieldMapping { get; set; }

        public SeperatedLineRule()
        {
            HasHeader = true.ToString();
            Seperator = ";";
            FieldMapping = string.Empty;
        }

        public override void ApplyTo(LineDocument lineDocument, long line)
        {
            var searchable = lineDocument.ToSearchable();
            var fields = searchable.Split(new[] {Seperator}, StringSplitOptions.None);
            if (Convert.ToBoolean(HasHeader) && line == 0)
            {
                lineDocument.WithHeaders(fields);
                return;
            }
            
            if (line == 0)
            {
                lineDocument.WithHeaders(Headers.Split(','));
            }

            for (var column = 0; column < lineDocument.Headers.Length; column++)
            {
                var header = lineDocument.Headers[column];
                var field = fields.Length > column ? fields[column] : string.Empty;
                lineDocument.AddField(header, field);
            }

            if (string.IsNullOrEmpty(FieldMapping))
            {
                lineDocument.Title = lineDocument.Headers.First();
                lineDocument.Content = lineDocument.Headers.Count() > 1 ? lineDocument.Headers[1] : string.Empty;
            }
            else
            {
                foreach (var mapPair in FieldMapping.Split(';'))
                {
                    var pair = mapPair.Split('=');
                    if (pair.Length != 2)
                        continue;

                    var key = pair[0];
                    var columns = pair[1].Split(',');
                    var values = new StringBuilder();
                    foreach (var value in 
                        columns.Select(lineDocument.TryGet).Where(_ => !string.IsNullOrEmpty(_)))
                    {
                        values.AppendLine(value);
                    }

                    var propertyInfo = typeof (LineDocument).GetProperty(key);
                    if (propertyInfo != null)
                        propertyInfo.SetValue(lineDocument, values.ToString());
                }
            }
        }
    }
}
