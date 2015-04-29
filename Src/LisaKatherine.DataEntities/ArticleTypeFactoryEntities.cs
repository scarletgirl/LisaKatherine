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
            ArticleTypeEntity articletype = (from a in this.dataModel.ArticleTypeEntity where a.articleTypeId == articleTypeId select a).First();

            return new ArticleType(articleTypeId, articletype.articleTypeName, articletype.sectionid);
        }

        public IEnumerable<IArticleType> GetList()
        {
            var list = new List<IArticleType>();

            foreach (ArticleTypeEntity a in this.dataModel.ArticleTypeEntity)
            {
                list.Add(new ArticleType(a.articleTypeId, a.articleTypeName, a.sectionid));
            }

            return list;
        }

        public bool Delete(int articleTypeId)
        {
            IQueryable<ArticleEntity> articleList = from a in this.dataModel.ArticleEntity where a.articleTypeId == articleTypeId select a;
            if (!articleList.Any())
            {
                ArticleTypeEntity originalArticleType = (from at in this.dataModel.ArticleTypeEntity where at.articleTypeId == articleTypeId select at).First();
                this.dataModel.DeleteObject(originalArticleType);
                this.dataModel.SaveChanges();
                return true;
            }

            return false;
        }

        public void Update(IArticleType articleType)
        {
            ArticleTypeEntity originalArticleType = (from at in this.dataModel.ArticleTypeEntity where at.articleTypeId == articleType.ArticleTypeId select at).First();

            ArticleTypeEntity art = ConvertToEntity(articleType);
            this.dataModel.ApplyCurrentValues(originalArticleType.EntityKey.EntitySetName, art);
            this.dataModel.SaveChanges();
        }

        public void Add(IArticleType articleType)
        {
            this.dataModel.AddToArticleTypeEntity(
                new ArticleTypeEntity { articleTypeId = articleType.ArticleTypeId, articleTypeName = articleType.ArticleTypeName, sectionid = articleType.SectionId });
            this.dataModel.SaveChanges();
        }

        private static ArticleTypeEntity ConvertToEntity(IArticleType articleType)
        {
            var art = new ArticleTypeEntity { articleTypeId = articleType.ArticleTypeId, articleTypeName = articleType.ArticleTypeName, sectionid = articleType.SectionId };
            return art;
        }
    }
}