using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Controllers
{
    public class BlogController : Controller
    {
        private readonly PublishedArticleService _publishedArticleService = new PublishedArticleService();
        private readonly ArticleService _articleService = new ArticleService();
        public ActionResult Index(int? id)
        {
            return View();
        }



        public PartialViewResult BlogList(int? page)
        {
            IEnumerable<PublishedArticles> articles = _publishedArticleService.GetPublishedList(1);
            PagedList<PublishedArticles> pa = new PagedList<PublishedArticles>(articles, page ?? 1, 5, articles.Count());
            return PartialView("_BlogList", pa);
        }

        public ActionResult Details(int id)
        {
            var article = _publishedArticleService.GetPublishedArticle(id);
            
            switch (article.articleType.sectionid)
            {
                case 2:
                    ViewBag.Section = "Geek";
                    ViewBag.Title = article.headline + " | Lisa Katherine Geekery";
                    break;
                case 3:
                    ViewBag.Section = "Work";
                    ViewBag.Title = article.headline + " | Lisa Katherine Work";
                    break;
                default:
                    ViewBag.Section = "Photos";
                    ViewBag.Title = article.headline + " | Lisa Katherine Photography";
                    break;
            }
            

            return View(article);
        }

        public ActionResult _BlogFeed()
        {
            IEnumerable<PublishedArticles> articles = _publishedArticleService.GetPublishedList(1).OrderByDescending(item => item.datePublished);
            return PartialView(articles);
        }

        public PartialViewResult RenderBlogFeed()
        {
            int[] x = { 1, 7, 8 };
            IEnumerable<PublishedArticles> articles = _publishedArticleService.GetPublishedArticlesManyTypes(new List<int>(x)).OrderByDescending(item => item.datePublished);
            return PartialView("_BlogFeed", articles);
        }

        public PartialViewResult RenderBlogJs(int? id)
        {

            if (id != null)
            {
                var article = _publishedArticleService.GetPublishedArticle((int)id);
                ViewBag.BlogMonth = String.Format("{0:yyMM}", article.datePublished);
                return PartialView("_BlogJs");
            }
            else
            {
                return null;
            }
        }

        public ActionResult Preview(int id) {
            var article = _articleService.GetArticle(id);
            ViewBag.Title = article.headline + "| Lisa Katherine ";
            ViewBag.Section = "Photos";

            return View(article);
        }

        public PartialViewResult LatestBlog(int id)
        {
            var article = _publishedArticleService.GetArticleLatestBlog(id);
            ViewBag.Section = Utils.GetSection(id);
            ViewBag.Image = "/Content/images/" + ViewBag.Section + "_sm.gif";
            article.body = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            return PartialView("_LatestBlog", article);
        }


        public ActionResult FBAuthorise(int id)
        {
            FBAuthHandler.handleOAuthResult(System.Web.HttpContext.Current.Request, id);
            return View();
        }


    }
}
