namespace LisaKatherine.DataEntitiesRepository
{
    using System.Collections.Generic;
    using System.Linq;

    using LisaKatherine.Interface;

    public class ArticleTypeFactoryEntities : IArticleTypeFactory
    {
        private readonly LisaKatherineEntities dataModel;

        public ArticleTypeFactoryEntities()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public ArticleTypeFactoryEntities(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public IArticleType Get(int articleTypeId)
        {
            ArticleTypes articletype =
                (from a in this.dataModel.ArticleTypes1 where a.articleTypeId == articleTypeId select a).First();
            return new ArticleType(articleTypeId, articletype.articleTypeName, articletype.sectionid);
        }

        public IEnumerable<IArticleType> GetList()
        {
            return
                this.dataModel.ArticleTypes1.OrderBy(a => a.articleTypeName)
                    .Select(aT => new ArticleType(aT.articleTypeId, aT.articleTypeName, aT.sectionid))
                    .ToList();
        }

        public bool Delete(int articleTypeId)
        {
            IQueryable<Articles> articleList = from a in this.dataModel.Articles1
                                               where a.articleTypeId == articleTypeId
                                               select a;
            if (!articleList.Any())
            {
                ArticleTypes originalArticleType =
                    (from at in this.dataModel.ArticleTypes1 where at.articleTypeId == articleTypeId select at).First();
                this.dataModel.DeleteObject(originalArticleType);
                this.dataModel.SaveChanges();
                return true;
            }

            return false;
        }

        public void Update(IArticleType articleType)
        {
            ArticleTypes originalArticleType =
                (from at in this.dataModel.ArticleTypes1 where at.articleTypeId == articleType.ArticleTypeId select at)
                    .First();

            this.dataModel.ApplyCurrentValues(originalArticleType.EntityKey.EntitySetName, articleType);
            this.dataModel.SaveChanges();
        }

        public void Add(IArticleType articleType)
        {
            this.dataModel.AddToArticleTypes1(
                new ArticleTypes
                    {
                        articleTypeId = articleType.ArticleTypeId,
                        articleTypeName = articleType.ArticleTypeName,
                        sectionid = articleType.SectionId
                    });
            this.dataModel.SaveChanges();
        }
    }
}