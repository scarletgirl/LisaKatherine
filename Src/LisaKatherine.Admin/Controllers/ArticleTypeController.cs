using System.Linq;
using System.Web.Mvc;
using LisaKatherine.Interface;
using LisaKatherine.Services;

namespace LisaKatherine.Admin.Controllers
{
    public class ArticleTypeController : Controller
    {
        private readonly ArticleTypeService articleTypeService = new ArticleTypeService();

        [Authorize]
        public ActionResult Index(string sortOrder)
        {
            var articleTypes = articleTypeService.GetArticleTypesList();
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "articleTypeId desc" : string.Empty;
            ViewBag.ArticleTypeSortParm = sortOrder == "articleTypeName" ? "articleTypeName desc" : "articleTypeName";

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
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(ArticleType articleType)
        {
            articleTypeService.CreateArticleType(articleType);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var articleType = articleTypeService.GetArticleType(id);
            if (articleType != null)
            {
                return View(articleType);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ArticleType articleType)
        {
            articleTypeService.EditArticleType(articleType);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var deleted = articleTypeService.DeleteArticleType(id);
            if (deleted)
            {
                return RedirectToAction("Index");
            }

            var articleType = articleTypeService.GetArticleType(id);
            return View(articleType);
        }
    }
}