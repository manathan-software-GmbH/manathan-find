namespace manathanFind.Services
{
    #region

    using manathan.find.Models;

    #endregion

    public interface ISearchService
    {
        HitCollection Search(string textToSearchFor);
        HitCollection Search(string textToSearchFor, string orderBy);
    }
}