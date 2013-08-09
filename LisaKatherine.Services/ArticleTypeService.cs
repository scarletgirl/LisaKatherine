namespace LisaKatherine.Services
{
    using System.Collections.Generic;

    using LisaKatherine.DataEntitiesRepository;
    using LisaKatherine.Interface;

    public class ArticleTypeService
    {
        private readonly IArticleTypeFactory articleTypeFactory;

        public ArticleTypeService(IArticleTypeFactory articleTypeFactory)
        {
            this.articleTypeFactory = articleTypeFactory;
        }

        public ArticleTypeService()
        {
            this.articleTypeFactory = new ArticleTypeFactoryEntities();
        }

        public IEnumerable<IArticleType> GetArticleTypesList()
        {
            return this.articleTypeFactory.GetList();
        }

        public void CreateArticleType(IArticleType articleType)
        {
            this.articleTypeFactory.Add(articleType);
        }

        public IArticleType GetArticleType(int id)
        {
            return this.articleTypeFactory.Get(id);
        }

        public void EditArticleType(IArticleType articleType)
        {
            this.articleTypeFactory.Update(articleType);
        }

        public bool DeleteArticleType(int id)
        {
           return this.articleTypeFactory.Delete(id);
        }
    }
}