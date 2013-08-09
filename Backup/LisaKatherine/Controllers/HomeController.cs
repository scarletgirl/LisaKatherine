using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class HomeController : Controller
    {
        readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();
        public ActionResult Index()
        {
            var article = _publishedArticleService.GetArticleByArticleType(2);
            if (article != null)
            {
                ViewBag.headline = article.headline;
                ViewBag.strapline = article.strapline;
                ViewBag.body = article.body;
            }
            ViewBag.ShowPartial = "Twitter";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.ShowPartial = "Twitter";
            ViewBag.Message = "About Me";
            return View();
        }

        public ActionResult Contact()
        {
            var article = _publishedArticleService.GetArticleByArticleType(5);
            ViewBag.ShowPartial = "Twitter";
            ViewBag.Message = "Contact";
            var cmv = new ContactViewModel();
            cmv.article = article;
            return View(cmv);
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel contactVM)
        {
            var article = _publishedArticleService.GetArticleByArticleType(5);
            ViewBag.ShowPartial = "Twitter";
            ViewBag.Message = "Contact";
            contactVM.article = article;
            if (!ModelState.IsValid)
            {
                return View(contactVM);
            }

            var contact = new Contact()
            {
                From = contactVM.From,
                Subject = contactVM.Subject,
                Message = contactVM.Message
            };

            new Email().Send(contact);

            return RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm()
        {
            return View();
        }

        public ActionResult Holding()
        {
            return View();
        }
    }
}
