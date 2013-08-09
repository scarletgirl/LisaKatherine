namespace LisaKatherine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ArticleTypeService
    {
        private readonly LisaKatherineEntities dataModel;

        private readonly UserService userService;

        public ArticleTypeService(LisaKatherineEntities dataModel, UserService userService)
        {
            this.dataModel = dataModel;
            this.userService = userService;
        }

        public ArticleTypeService()
        {
            this.dataModel = new LisaKatherineEntities();
            this.userService = new UserService();
        }

        public IEnumerable<ArticleTypes> GetArticleTypesList()
        {
            return this.dataModel.ArticleTypes1.OrderBy(a => a.articleTypeName);
        }

        public void CreateArticleType(ArticleTypes articleType)
        {
            this.dataModel.AddToArticleTypes1(articleType);
            this.dataModel.SaveChanges();
        }

        public ArticleTypes GetArticleType(int id)
        {
            ArticleTypes articletype =
                (from a in this.dataModel.ArticleTypes1 where a.articleTypeId == id select a).First();
            return articletype;
        }

        public void EditArticleType(ArticleTypes articleType)
        {
            ArticleTypes originalArticleType =
                (from at in this.dataModel.ArticleTypes1 where at.articleTypeId == articleType.articleTypeId select at)
                    .First();

            this.dataModel.ApplyCurrentValues(originalArticleType.EntityKey.EntitySetName, articleType);
            this.dataModel.SaveChanges();
        }

        public Boolean DeleteArticleType(int id)
        {
            IQueryable<Articles> articleList = from a in this.dataModel.Articles1 where a.articleTypeId == id select a;
            if (!articleList.Any())
            {
                ArticleTypes originalArticleType =
                    (from at in this.dataModel.ArticleTypes1 where at.articleTypeId == id select at).First();
                this.dataModel.DeleteObject(originalArticleType);
                this.dataModel.SaveChanges();
                return true;
            }

            return false;
        }
    }
}