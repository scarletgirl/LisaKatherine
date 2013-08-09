using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;
using System.Globalization;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ArticleService _articleService = new ArticleService();

        [Authorize]
        public ActionResult Index(string sortOrder, int? id)
        {
            ViewBag.HeadlineSortParm = String.IsNullOrEmpty(sortOrder) ? "headline desc" : "";
            ViewBag.CreatedSortParm = sortOrder == "dateCreated" ? "dateCreated desc" : "dateCreated";
            ViewBag.PublishedSortParm = sortOrder == "datePublished" ? "datePublished desc" : "datePublished";
            ViewBag.IsPublishedSortParm = sortOrder == "isPublished" ? "isPublished desc" : "isPublished";
            ViewBag.ArticleTypeSortParm = sortOrder == "articleType" ? "articleType desc" : "articleType";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author desc" : "author";

            IEnumerable<Articles> articles = _articleService.GetList(0);

            switch (sortOrder)
            {
                case "headline desc": articles = articles.OrderByDescending(a => a.headline); break;
                case "dateCreated": articles = articles.OrderBy(a => a.dateCreated); break;
                case "dateCreated desc": articles = articles.OrderByDescending(a => a.dateCreated); break;
                case "datePublished": articles = articles.OrderBy(a => a.datePublished); break;
                case "datePublished desc": articles = articles.OrderByDescending(a => a.datePublished); break;
                case "isPublished": articles = articles.OrderBy(a => a.isPublished); break;
                case "isPublished desc": articles = articles.OrderByDescending(a => a.isPublished); break;
                case "articleType": articles = articles.OrderBy(a => a.articleType.articleTypeName); break;
                case "articleType desc": articles = articles.OrderByDescending(a => a.articleType.articleTypeName); break;
                case "author": articles = articles.OrderBy(a => a.user.firstname); break;
                case "author desc": articles = articles.OrderByDescending(a => a.user.firstname); break;
                default: articles = articles.OrderBy(a => a.headline); break;
            }
            PagedList<Articles> pagedarticles = new PagedList<Articles>(articles, id ?? 1, Settings.AdminPagerCount(), articles.Count());
            return View(pagedarticles);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewData["articleTypeId_0"] = new SelectList(CreateArticleTypeList(0), "Value", "Text");
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, string submitButton)
        {
            ViewData["articleTypeId_0"] = new SelectList(CreateArticleTypeList(0), "Value", "Text");
            try
            {
                var article = new Articles()
                {

                    articleTypeId = Convert.ToInt32(form["articleTypeId_0"]),
                    body = form["Body"],
                    dateCreated = DateTime.Now,
                    headline = form["headline"],
                    strapline = form["strapline"]
                };

                _articleService.CreateArticle(article);
                Publish(form, submitButton, article);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var article = _articleService.GetArticle(id);
            ViewData["articleTypeId_0"] = new SelectList(CreateArticleTypeList(article.articleTypeId), "Value", "Text", article.articleTypeId);
            return View(article);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, string submitButton)
        {
            var article = new Articles()
            {
                articleTypeId = Convert.ToInt32(form["articleTypeId_0"]),
                articleId = Convert.ToInt32(form["articleId"]),
                body = form["Body"],
                dateCreated = DateTime.Now,
                headline = form["headline"],
                strapline = form["strapline"],
                userid = new Guid(form["userid"])
            };
            Publish(form, submitButton, article);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var article = _articleService.GetArticle(id);
            return View(article);
        }


        /*** Functions ***/

        public List<SelectListItem> CreateArticleTypeList(int? articleTypeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (ArticleTypes item in new ArticleTypeService().GetArticleTypesList())
            {

                items.Add(new SelectListItem { Text = item.articleTypeName, Value = item.articleTypeId.ToString() });
            }
            return items;
        }
        private static DateTime GetDatePublished(FormCollection form)
        {
            if (form["datePublished"] != String.Empty)
            {
                CultureInfo culture = new CultureInfo("en-GB");
                return Convert.ToDateTime(form["datePublished"], culture);
            }
            return DateTime.Now;
        }
        private void Publish(FormCollection form, string submitButton, Articles article)
        {
            if (submitButton.Contains("Publish"))
            {
                article.datePublished = GetDatePublished(form);
                article.isPublished = true;
                _articleService.PublishArticle(article);
            }
            _articleService.EditArticle(article);
        }
    }
}
