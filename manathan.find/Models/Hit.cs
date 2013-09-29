namespace manathan.find.Models
{
    #region

    using System;

    #endregion

    public class Hit
    {
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DateTime { get; set; }
        public HitCollection RelatedHits { get; set; }
    }
}