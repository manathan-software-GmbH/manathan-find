namespace manathanFind.Controllers
{
    #region

    using System.Web.Mvc;
    using System.Web.Routing;
    using Services;

    #endregion

    public class HomeController : Controller
    {
        ISearchService searchService;

        protected override void Initialize(RequestContext requestContext)
        {
            searchService = new SearchService();
            base.Initialize(requestContext);
        }

        public ActionResult Index(string q, string o)
        {
            ViewData["o"] = o;
            return View(searchService.Search(q, o));
        }
    }
}