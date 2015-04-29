namespace LisaKatherine.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class ArticleTypeController : Controller
    {
        private readonly ArticleTypeService articleTypeService = new ArticleTypeService();

        [Authorize]
        public ActionResult Index(string sortOrder)
        {
            IEnumerable<IArticleType> articleTypes = this.articleTypeService.GetArticleTypesList();
            this.ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "articleTypeId desc" : "";
            this.ViewBag.ArticleTypeSortParm = sortOrder == "articleTypeName" ? "articleTypeName desc" : "articleTypeName";

            switch (sortOrder)
            {
                case "articleTypeId desc":
                    articleTypes = articleTypes.OrderByDescending(a => a.ArticleTypeId);
                    break;
                case "articleTypeName":
                    articleTypes = articleTypes.OrderBy(a => a.ArticleTypeName);
                    break;
                case "articleTypeName desc":
                    articleTypes = articleTypes.OrderByDescending(a => a.ArticleTypeName);
                    break;

                default:
                    articleTypes = articleTypes.OrderBy(a => a.ArticleTypeId);
                    break;
            }
            return this.View(articleTypes);
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(ArticleType articleType)
        {
            this.articleTypeService.CreateArticleType(articleType);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            IArticleType articleType = this.articleTypeService.GetArticleType(id);
            if (articleType != null)
            {
                return this.View(articleType);
            }
            else
            {
                return this.RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ArticleType articleType)
        {
            this.articleTypeService.EditArticleType(articleType);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Boolean deleted = this.articleTypeService.DeleteArticleType(id);
            if (deleted)
            {
                return this.RedirectToAction("Index");
            }

            IArticleType articleType = this.articleTypeService.GetArticleType(id);
            return this.View(articleType);
        }
    }
}