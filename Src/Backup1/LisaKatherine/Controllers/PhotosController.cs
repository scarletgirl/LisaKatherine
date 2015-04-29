namespace LisaKatherine.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    using Webdiyer.WebControls.Mvc;

    public class PhotosController : Controller
    {
        //
        // GET: /Photos/
        private readonly PublishedArticleService publishedArticleService = new PublishedArticleService();

        private readonly PhotoService photoService = new PhotoService();

        public ActionResult Index()
        {
            IPublishedArticle article = this.publishedArticleService.GetArticleByArticleType(10);
            if (article != null)
            {
                this.ViewBag.Headline = article.Headline;
                this.ViewBag.Strapline = article.Strapline;
                this.ViewBag.Body = article.Body;
            }
            return this.View();
        }

        public ActionResult PhotoSet(long id)
        {
            return this.View();
        }

        public PartialViewResult RenderFlickrSetList(int? id)
        {
            IEnumerable<IPhotoSet> flickrSets = this.photoService.GetList();
            var fs = new PagedList<IPhotoSet>(flickrSets, id ?? 1, 5, flickrSets.Count());

            return this.PartialView("_FlickrSets", fs);
        }

        public PartialViewResult BlogList(int? page)
        {
            return this.PartialView("_BlogList", this.publishedArticleService.GetBlogList(1, page));
        }
    }
}