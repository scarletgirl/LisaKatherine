using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LisaKatherine.Models;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Controllers
{
    public class PhotosController : Controller
    {
        //
        // GET: /Photos/
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();
        private readonly FlickrImageService _flickrImageService = new FlickrImageService();
        public ActionResult Index()
        {
            var article = _publishedArticleService.GetArticleByArticleType(10);
            if (article != null)
            {
                ViewBag.headline = article.headline;
                ViewBag.strapline = article.strapline;
                ViewBag.body = article.body;
            }
            return View();
        }

        public ActionResult PhotoSet(long id) {
            return View();
        }

        public PartialViewResult RenderFlickrSetList(int? id)
        {
            IEnumerable<FlickrSets> flickrSets = _flickrImageService.GetList();
            PagedList<FlickrSets> fs = new PagedList<FlickrSets>(flickrSets, id ?? 1, 5, flickrSets.Count());

            return PartialView("_FlickrSets", fs);
        }

        public PartialViewResult BlogList(int? page) { 
            return PartialView("_BlogList", _publishedArticleService.GetBlogList(1, page));
        }

    }
}
