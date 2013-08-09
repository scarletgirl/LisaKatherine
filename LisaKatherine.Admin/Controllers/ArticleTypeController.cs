namespace LisaKatherine.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class ArticleTypeController : Controller
    {
        private readonly ArticleTypeService _articleTypeService = new ArticleTypeService();

        [Authorize]
        public ActionResult Index(string sortOrder)
        {
            IEnumerable<IArticleType> articleTypes = this._articleTypeService.GetArticleTypesList();
            this.ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "articleTypeId desc" : "";
            this.ViewBag.ArticleTypeSortParm = sortOrder == "articleTypeName"
                                                   ? "articleTypeName desc"
                                                   : "articleTypeName";

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
            return View(articleTypes);
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(IArticleType articleType)
        {
            this._articleTypeService.CreateArticleType(articleType);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            IArticleType articleType = this._articleTypeService.GetArticleType(id);
            if (articleType != null)
            {
                return View(articleType);
            }
            else
            {
                return this.RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IArticleType articleType)
        {
            this._articleTypeService.EditArticleType(articleType);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Boolean deleted = this._articleTypeService.DeleteArticleType(id);
            if (deleted)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                IArticleType articleType = this._articleTypeService.GetArticleType(id);
                return View(articleType);
            }
        }
    }
}