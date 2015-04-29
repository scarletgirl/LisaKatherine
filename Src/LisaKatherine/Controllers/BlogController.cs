using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LisaKatherine.Interface;
using LisaKatherine.Services;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Controllers
{
    public class BlogController : Controller
    {
        private readonly ArticleService articleService = new ArticleService();
        private readonly PublishedArticleService publishedArticleService = new PublishedArticleService();

        public ActionResult Index(int? id)
        {
            return View();
        }

        public PartialViewResult BlogList(int? page)
        {
            var articles = publishedArticleService.GetPublishedList(1);
            var pa = new PagedList<IPublishedArticle>(articles, page ?? 1, 5, articles.Count());
            return PartialView("_BlogList", pa);
        }

        public ActionResult Details(int id)
        {
            var article = publishedArticleService.GetPublishedArticle(id);

            switch (article.ArticleType.SectionId)
            {
                case 2:
                    ViewBag.Section = "Geek";
                    ViewBag.Title = article.Headline + " | Lisa Katherine Geekery";
                    break;
                case 3:
                    ViewBag.Section = "Work";
                    ViewBag.Title = article.Headline + " | Lisa Katherine Work";
                    break;
                default:
                    ViewBag.Section = "Photos";
                    ViewBag.Title = article.Headline + " | Lisa Katherine Photography";
                    break;
            }

            return View(article);
        }



        public ActionResult _BlogFeed()
        {
            IEnumerable<IPublishedArticle> articles =
                publishedArticleService.GetPublishedList(1).OrderByDescending(item => item.DatePublished);
            return PartialView(articles);
        }

        public PartialViewResult RenderBlogFeed()
        {
            int[] x = { 1, 7, 8 };
            IEnumerable<IPublishedArticle> articles =
                publishedArticleService.GetPublishedArticlesManyTypes(new List<int>(x))
                    .OrderByDescending(item => item.DatePublished);
            return PartialView("_BlogFeed", articles);
        }

        public PartialViewResult RenderBlogJs(int? id)
        {
            if (id != null)
            {
                var article = publishedArticleService.GetPublishedArticle((int) id);
                ViewBag.BlogMonth = String.Format("{0:yyMM}", article.DatePublished);
                return PartialView("_BlogJs");
            }
            return null;
        }

        public ActionResult Preview(int id)
        {
            var article = articleService.GetArticle(id);
            ViewBag.Title = article.Headline + "| Lisa Katherine ";
            ViewBag.Section = "Photos";

            return View(article);
        }

        public PartialViewResult LatestBlog(int id)
        {
            var article = publishedArticleService.GetArticleLatestBlog(id);
            ViewBag.Section = Utils.GetSection(id);
            ViewBag.Image = "/Content/images/" + ViewBag.Section + "_sm.gif";
            article.Body = Utils.GetSummary(Utils.StripHtml(article.Body), 255);
            return PartialView("_LatestBlog", article);
        }

        public ActionResult FbAuthorise(int id)
        {
            //FbAuthHandler.HandleOAuthResult(System.Web.HttpContext.Current.Request, id);
            return View();
        }
    }
}