namespace manathan.find.Models
{
    #region

    using System.Collections.Generic;

    #endregion

    public class HitCollection : List<Hit>
    {
        public HitCollection()
        {
        }

        public HitCollection(IEnumerable<Hit> hits)
        {
            AddRange(hits);
        }
    }
}