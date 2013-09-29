namespace manathan.find.Crawler
{
    #region

    using System;

    #endregion

    public abstract class BaseDocument
    {
        public virtual string Title { get; set; }
        public virtual string MetaContext { get; set; }
        public virtual string Content { get; set; }
        public virtual Uri Uri { get; set; }
        public virtual DateTime Date { get; set; }

        public abstract string ToSearchable();
    }
}