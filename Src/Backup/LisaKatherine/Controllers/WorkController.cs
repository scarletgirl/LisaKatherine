using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class WorkController : Controller
    {
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();

        public ActionResult Index()
        {
            var workArticle = _publishedArticleService.GetArticleByArticleType(6);
            return View(workArticle);
        }

        public PartialViewResult BlogList(int? page)
        {
            return PartialView("_BlogList", _publishedArticleService.GetBlogList(8, page));
        }

    }
}
