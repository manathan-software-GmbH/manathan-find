namespace manathanFind.Services
{
    #region

    using manathan.find;
    using manathan.find.Models;

    #endregion

    public class SearchService : ISearchService
    {
        public HitCollection Search(string textToSearchFor)
        {
            return Search(textToSearchFor, "");
        }

        public HitCollection Search(string textToSearchFor, string orderBy)
        {
            return string.IsNullOrEmpty(textToSearchFor)
                       ? new HitCollection()
                       : new SearchEngine().Search(textToSearchFor, orderBy);
        }
    }
}