namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class WorkController : Controller
    {
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();

        public ActionResult Index()
        {
            IPublishedArticle workArticle = this._publishedArticleService.GetArticleByArticleType(6);
            return View(workArticle);
        }

        public PartialViewResult BlogList(int? page)
        {
            return this.PartialView("_BlogList", this._publishedArticleService.GetBlogList(8, page));
        }
    }
}