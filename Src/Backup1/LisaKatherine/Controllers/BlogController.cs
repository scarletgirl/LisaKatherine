namespace LisaKatherine.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    using Webdiyer.WebControls.Mvc;

    public class BlogController : Controller
    {
        private readonly PublishedArticleService publishedArticleService = new PublishedArticleService();

        private readonly ArticleService articleService = new ArticleService();

        public ActionResult Index(int? id)
        {
            return this.View();
        }

        public PartialViewResult BlogList(int? page)
        {
            IEnumerable<IPublishedArticle> articles = this.publishedArticleService.GetPublishedList(1);
            var pa = new PagedList<IPublishedArticle>(articles, page ?? 1, 5, articles.Count());
            return this.PartialView("_BlogList", pa);
        }

        public ActionResult Details(int id)
        {
            IPublishedArticle article = this.publishedArticleService.GetPublishedArticle(id);

            switch (article.ArticleType.SectionId)
            {
                case 2:
                    this.ViewBag.Section = "Geek";
                    this.ViewBag.Title = article.Headline + " | Lisa Katherine Geekery";
                    break;
                case 3:
                    this.ViewBag.Section = "Work";
                    this.ViewBag.Title = article.Headline + " | Lisa Katherine Work";
                    break;
                default:
                    this.ViewBag.Section = "Photos";
                    this.ViewBag.Title = article.Headline + " | Lisa Katherine Photography";
                    break;
            }

            return View(article);
        }

        public ActionResult _BlogFeed()
        {
            IEnumerable<IPublishedArticle> articles =
                this.publishedArticleService.GetPublishedList(1).OrderByDescending(item => item.DatePublished);
            return PartialView(articles);
        }

        public PartialViewResult RenderBlogFeed()
        {
            int[] x = { 1, 7, 8 };
            IEnumerable<IPublishedArticle> articles =
                this.publishedArticleService.GetPublishedArticlesManyTypes(new List<int>(x))
                    .OrderByDescending(item => item.DatePublished);
            return this.PartialView("_BlogFeed", articles);
        }

        public PartialViewResult RenderBlogJs(int? id)
        {
            if (id != null)
            {
                IPublishedArticle article = this.publishedArticleService.GetPublishedArticle((int)id);
                this.ViewBag.BlogMonth = String.Format("{0:yyMM}", article.DatePublished);
                return this.PartialView("_BlogJs");
            }
            else
            {
                return null;
            }
        }

        public ActionResult Preview(int id)
        {
            IArticle article = this.articleService.GetArticle(id);
            this.ViewBag.Title = article.Headline + "| Lisa Katherine ";
            this.ViewBag.Section = "Photos";

            return View(article);
        }

        public PartialViewResult LatestBlog(int id)
        {
            IPublishedArticle article = this.publishedArticleService.GetArticleLatestBlog(id);
            this.ViewBag.Section = Utils.GetSection(id);
            this.ViewBag.Image = "/Content/images/" + this.ViewBag.Section + "_sm.gif";
            article.Body = Utils.GetSummary(Utils.StripHtml(article.Body), 255);
            return this.PartialView("_LatestBlog", article);
        }

        public ActionResult FbAuthorise(int id)
        {
            //FbAuthHandler.HandleOAuthResult(System.Web.HttpContext.Current.Request, id);
            return this.View();
        }
    }
}