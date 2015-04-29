namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Services;

    public class GeekController : Controller
    {
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();

        public ActionResult Index()
        {
            var article = this._publishedArticleService.GetArticleByArticleType(9);
            if (article != null)
            {
                this.ViewBag.Headline = article.Headline;
                this.ViewBag.Strapline = article.Strapline;
                this.ViewBag.Body = article.Body;
            }
            return this.View();
        }

        public PartialViewResult BlogList(int? page)
        {
            return this.PartialView("_BlogList", this._publishedArticleService.GetBlogList(7, page));
        }
    }
}