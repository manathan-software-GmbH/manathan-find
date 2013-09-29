namespace manathan.file.crawler.Rules
{
    public abstract class LineRule
    {
        public string FieldIndex { get; set; }

        public abstract void ApplyTo(LineDocument lineDocument, long line);
    }
}