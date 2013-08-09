using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class ArticleTypeController : Controller
    {
        private readonly ArticleTypeService _articleTypeService = new ArticleTypeService();

        [Authorize]
        public ActionResult Index(string sortOrder)
        {
            var articleTypes = _articleTypeService.GetArticleTypesList();
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "articleTypeId desc" : "";
            ViewBag.ArticleTypeSortParm = sortOrder == "articleTypeName" ? "articleTypeName desc" : "articleTypeName";

            switch (sortOrder)
            {
                case "articleTypeId desc": articleTypes = articleTypes.OrderByDescending(a => a.articleTypeId); break;
                case "articleTypeName": articleTypes = articleTypes.OrderBy(a => a.articleTypeName); break;
                case "articleTypeName desc": articleTypes = articleTypes.OrderByDescending(a => a.articleTypeName); break;

                default: articleTypes = articleTypes.OrderBy(a => a.articleTypeId); break;
            }
            return View(articleTypes);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(ArticleTypes articleType)
        {

            _articleTypeService.CreateArticleType(articleType);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var articleType = _articleTypeService.GetArticleType(id);
            if (articleType != null)
            {
                return View(articleType);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ArticleTypes articleType)
        {
            _articleTypeService.EditArticleType(articleType);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Boolean deleted = _articleTypeService.DeleteArticleType(id);
            if (deleted)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var articleType = _articleTypeService.GetArticleType(id);
                return View(articleType);
            }

        }


    }
}
