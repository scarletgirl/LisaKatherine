namespace LisaKatherine.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Models;
    using LisaKatherine.Models.Extensions;

    using Webdiyer.WebControls.Mvc;

    public class BlogController : Controller
    {
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();

        private readonly ArticleService _articleService = new ArticleService();

        public ActionResult Index(int? id)
        {
            return this.View();
        }

        public PartialViewResult BlogList(int? page)
        {
            IEnumerable<PublishedArticles> articles = this._publishedArticleService.GetPublishedList(1);
            var pa = new PagedList<PublishedArticles>(articles, page ?? 1, 5, articles.Count());
            return this.PartialView("_BlogList", pa);
        }

        public ActionResult Details(int id)
        {
            PublishedArticles article = this._publishedArticleService.GetPublishedArticle(id);

            switch (article.ArticleType.sectionid)
            {
                case 2:
                    this.ViewBag.Section = "Geek";
                    this.ViewBag.Title = article.headline + " | Lisa Katherine Geekery";
                    break;
                case 3:
                    this.ViewBag.Section = "Work";
                    this.ViewBag.Title = article.headline + " | Lisa Katherine Work";
                    break;
                default:
                    this.ViewBag.Section = "Photos";
                    this.ViewBag.Title = article.headline + " | Lisa Katherine Photography";
                    break;
            }

            return View(article);
        }

        public ActionResult _BlogFeed()
        {
            IEnumerable<PublishedArticles> articles =
                this._publishedArticleService.GetPublishedList(1).OrderByDescending(item => item.datePublished);
            return PartialView(articles);
        }

        public PartialViewResult RenderBlogFeed()
        {
            int[] x = { 1, 7, 8 };
            IEnumerable<PublishedArticles> articles =
                this._publishedArticleService.GetPublishedArticlesManyTypes(new List<int>(x))
                    .OrderByDescending(item => item.datePublished);
            return this.PartialView("_BlogFeed", articles);
        }

        public PartialViewResult RenderBlogJs(int? id)
        {
            if (id != null)
            {
                PublishedArticles article = this._publishedArticleService.GetPublishedArticle((int)id);
                this.ViewBag.BlogMonth = String.Format("{0:yyMM}", article.datePublished);
                return this.PartialView("_BlogJs");
            }
            else
            {
                return null;
            }
        }

        public ActionResult Preview(int id)
        {
            IArticle article = this._articleService.GetArticle(id);
            this.ViewBag.Title = article.Headline + "| Lisa Katherine ";
            this.ViewBag.Section = "Photos";

            return View(article);
        }

        public PartialViewResult LatestBlog(int id)
        {
            PublishedArticles article = this._publishedArticleService.GetArticleLatestBlog(id);
            this.ViewBag.Section = Utils.GetSection(id);
            this.ViewBag.Image = "/Content/images/" + this.ViewBag.Section + "_sm.gif";
            article.body = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            return this.PartialView("_LatestBlog", article);
        }

        public ActionResult FBAuthorise(int id)
        {
            FbAuthHandler.HandleOAuthResult(System.Web.HttpContext.Current.Request, id);
            return this.View();
        }
    }
}