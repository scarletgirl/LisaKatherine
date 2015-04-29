using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using LisaKatherine.Interface;
using LisaKatherine.Services;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Admin.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ArticleService articleService = new ArticleService();
        private readonly ArticleTypeService articleTypeService = new ArticleTypeService();
        private readonly UserService userFactory = new UserService();

        [Authorize]
        public ActionResult Index(string sortOrder, int? id)
        {
            if (id == null)
            {
                id = 1;
            }

            ViewBag.HeadlineSortParm = string.IsNullOrEmpty(sortOrder) ? "headline desc" : string.Empty;
            ViewBag.CreatedSortParm = sortOrder == "dateCreated" ? "dateCreated desc" : "dateCreated";
            ViewBag.PublishedSortParm = sortOrder == "datePublished" ? "datePublished desc" : "datePublished";
            ViewBag.IsPublishedSortParm = sortOrder == "isPublished" ? "isPublished desc" : "isPublished";
            ViewBag.ArticleTypeSortParm = sortOrder == "articleType" ? "articleType desc" : "articleType";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author desc" : "author";

            var pagedarticles = GetPaging(sortOrder, id);
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
            ViewData["ArticleTypeId_0"] = new SelectList(CreateArticleTypeList(0), "Value", "Text");
            try
            {
                var articleType = articleTypeService.GetArticleType(Convert.ToInt32(form["ArticleTypeId_0"]));
                var user = userFactory.CheckSession();
                var article = new Article(form["Headline"], form["Strapline"], form["Body"], DateTime.Now, null, false, articleType.ArticleTypeId, user, articleType);
                articleService.CreateArticle(article);
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
            var article = articleService.GetArticle(id);
            ViewData["ArticleTypeId_0"] = new SelectList(CreateArticleTypeList(article.ArticleTypeId), "Value", "Text", article.ArticleTypeId);
            return View(article);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, string submitButton)
        {
            var articleType = articleTypeService.GetArticleType(Convert.ToInt32(form["ArticleTypeId_0"]));
            var user = userFactory.GetUser(new Guid(form["User.UserId"]));
            var article = new Article(form["Headline"], form["Strapline"], form["Body"], DateTime.Now, null, false, articleType.ArticleTypeId, user, articleType)
            {
                ArticleId = Convert.ToInt32(form["ArticleId"]),
                Userid = user.UserId
            };

            Publish(form, submitButton, article);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            articleService.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        /*** Functions ***/

        public List<SelectListItem> CreateArticleTypeList(int? articleTypeId)
        {
            return
                articleTypeService.GetArticleTypesList()
                    .Select(item => new SelectListItem { Text = item.ArticleTypeName, Value = item.ArticleTypeId.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }

        private static DateTime GetDatePublished(NameValueCollection form)
        {
            if (form["DatePublished"] != string.Empty)
            {
                var culture = new CultureInfo("en-GB");
                return Convert.ToDateTime(form["DatePublished"], culture);
            }

            return DateTime.Now;
        }

        private PagedList<IArticle> GetPaging(string sortOrder, int? id)
        {
            if (id == null)
            {
                id = 0;
            }

            var articles = articleService.GetList((int)id);

            switch (sortOrder)
            {
                case "headline desc":
                    articles = articles.OrderByDescending(a => a.Headline);
                    break;
                case "dateCreated":
                    articles = articles.OrderBy(a => a.DateCreated);
                    break;
                case "dateCreated desc":
                    articles = articles.OrderByDescending(a => a.DateCreated);
                    break;
                case "datePublished":
                    articles = articles.OrderBy(a => a.DatePublished);
                    break;
                case "datePublished desc":
                    articles = articles.OrderByDescending(a => a.DatePublished);
                    break;
                case "isPublished":
                    articles = articles.OrderBy(a => a.IsPublished);
                    break;
                case "isPublished desc":
                    articles = articles.OrderByDescending(a => a.IsPublished);
                    break;
                case "articleType":
                    articles = articles.OrderBy(a => a.ArticleType.ArticleTypeName);
                    break;
                case "articleType desc":
                    articles = articles.OrderByDescending(a => a.ArticleType.ArticleTypeName);
                    break;
                case "author":
                    articles = articles.OrderBy(a => a.User.FirstName);
                    break;
                case "author desc":
                    articles = articles.OrderByDescending(a => a.User.FirstName);
                    break;
                default:
                    articles = articles.OrderBy(a => a.Headline);
                    break;
            }

            var pagedarticles = new PagedList<IArticle>(articles, id ?? 1, Settings.AdminPagerCount(), articles.Count());
            return pagedarticles;
        }

        private void Publish(FormCollection form, string submitButton, Article article)
        {
            articleService.EditArticle(article);
            if (submitButton.Contains("Publish"))
            {
                article.DatePublished = GetDatePublished(form);
                article.IsPublished = true;
                articleService.PublishArticle(article);
            }
        }
    }
}