namespace LisaKatherine.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Models;
    using LisaKatherine.Services;

    using Webdiyer.WebControls.Mvc;

    public class ArticlesController : Controller
    {
        private readonly ArticleService articleService = new ArticleService();

        private readonly ArticleTypeService articleTypeService = new ArticleTypeService();

        private readonly UserService userFactory = new UserService();

        [Authorize]
        public ActionResult Index(string sortOrder, int? id)
        {
            this.ViewBag.HeadlineSortParm = String.IsNullOrEmpty(sortOrder) ? "headline desc" : "";
            this.ViewBag.CreatedSortParm = sortOrder == "dateCreated" ? "dateCreated desc" : "dateCreated";
            this.ViewBag.PublishedSortParm = sortOrder == "datePublished" ? "datePublished desc" : "datePublished";
            this.ViewBag.IsPublishedSortParm = sortOrder == "isPublished" ? "isPublished desc" : "isPublished";
            this.ViewBag.ArticleTypeSortParm = sortOrder == "articleType" ? "articleType desc" : "articleType";
            this.ViewBag.AuthorSortParm = sortOrder == "author" ? "author desc" : "author";

            PagedList<IArticle> pagedarticles = this.GetPaging(sortOrder, id);
            return this.View(pagedarticles);
        }

        [Authorize]
        public ActionResult Create()
        {
            this.ViewData["articleTypeId_0"] = new SelectList(this.CreateArticleTypeList(0), "Value", "Text");
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, string submitButton)
        {
            this.ViewData["articleTypeId_0"] = new SelectList(this.CreateArticleTypeList(0), "Value", "Text");
            try
            {
                IArticleType articleType =
                    this.articleTypeService.GetArticleType(Convert.ToInt32(form["articleTypeId_0"]));
                IUser user = this.userFactory.CheckSession();
                var article = new Article(
                    form["headline"],
                    form["strapline"],
                    form["Body"],
                    DateTime.Now,
                    null,
                    false,
                    articleType.ArticleTypeId,
                    user,
                    articleType);
                this.articleService.CreateArticle(article);
                this.Publish(form, submitButton, article);
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            IArticle article = this.articleService.GetArticle(id);
            this.ViewData["articleTypeId_0"] = new SelectList(
                this.CreateArticleTypeList(article.ArticleTypeId), "Value", "Text", article.ArticleTypeId);
            return this.View(article);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, string submitButton)
        {
            IArticleType articleType = this.articleTypeService.GetArticleType(Convert.ToInt32(form["articleTypeId_0"]));
            IUser user = this.userFactory.GetUser(new Guid(form["userid"]));
            var article = new Article(
                form["headline"],
                form["strapline"],
                form["Body"],
                DateTime.Now,
                null,
                false,
                articleType.ArticleTypeId,
                user,
                articleType) { ArticleId = Convert.ToInt32(form["articleId"]) };
            this.Publish(form, submitButton, article);

            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            this.articleService.DeleteArticle(id);
            return this.RedirectToAction("Index");
        }

        /*** Functions ***/

        public List<SelectListItem> CreateArticleTypeList(int? articleTypeId)
        {
            return
                this.articleTypeService.GetArticleTypesList()
                    .Select(
                        item =>
                        new SelectListItem
                            {
                                Text = item.ArticleTypeName,
                                Value = item.ArticleTypeId.ToString(CultureInfo.InvariantCulture)
                            })
                    .ToList();
        }

        private static DateTime GetDatePublished(FormCollection form)
        {
            if (form["datePublished"] != String.Empty)
            {
                var culture = new CultureInfo("en-GB");
                return Convert.ToDateTime(form["datePublished"], culture);
            }
            return DateTime.Now;
        }

        private PagedList<IArticle> GetPaging(string sortOrder, int? id)
        {
            IEnumerable<IArticle> articles = this.articleService.GetList(0);

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
            if (submitButton.Contains("Publish"))
            {
                article.DatePublished = GetDatePublished(form);
                article.IsPublished = true;
                this.articleService.PublishArticle(article);
            }
            this.articleService.EditArticle(article);
        }
    }
}