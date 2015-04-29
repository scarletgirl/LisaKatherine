using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LisaKatherine.Models;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Controllers
{
    public class GeekController : Controller
    {

        readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();
        public ActionResult Index()
        {
            var article = _publishedArticleService.GetArticleByArticleType(9);
            if (article != null)
            {
                ViewBag.headline = article.headline;
                ViewBag.strapline = article.strapline;
                ViewBag.body = article.body;
            }
            return View();
        }

        public PartialViewResult BlogList(int? page)
        {
            return PartialView("_BlogList", _publishedArticleService.GetBlogList(7, page));
        }

    }
}
