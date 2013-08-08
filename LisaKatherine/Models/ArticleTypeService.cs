using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LisaKatherine.Models
{
    public class ArticleTypeService
    {
        private readonly LisaKatherineEntities _dataModel;
        private readonly UserService _userService;

        public ArticleTypeService(LisaKatherineEntities dataModel, UserService userService)
        {
            _dataModel = dataModel;
            _userService = userService;
        }

        public ArticleTypeService()
        {
            _dataModel = new LisaKatherineEntities();
            _userService = new UserService();
        }

        public IEnumerable<ArticleTypes> GetArticleTypesList()
        {

            return _dataModel.ArticleTypes1.OrderBy(a => a.articleTypeName);
        }

        public void CreateArticleType(ArticleTypes articleType)
        {
            _dataModel.AddToArticleTypes1(articleType);
            _dataModel.SaveChanges();
        }

        public ArticleTypes GetArticleType(int id)
        {
            var articletype = (from a in _dataModel.ArticleTypes1 where a.articleTypeId == id select a).First();
            return articletype;
        }

        public void EditArticleType(ArticleTypes articleType)
        {
            var originalArticleType = (from at in _dataModel.ArticleTypes1 where at.articleTypeId == articleType.articleTypeId select at).First();

            _dataModel.ApplyCurrentValues(originalArticleType.EntityKey.EntitySetName, articleType);
            _dataModel.SaveChanges();
        }

        public Boolean DeleteArticleType(int id)
        {
            var articleList = from a in _dataModel.Articles1 where a.articleTypeId == id select a;
            if (articleList.Count() == 0)
            {
                var originalArticleType = (from at in _dataModel.ArticleTypes1 where at.articleTypeId == id select at).First();
                _dataModel.DeleteObject(originalArticleType);
                _dataModel.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}