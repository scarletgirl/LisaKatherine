namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface IArticleFactory
    {
        IArticle Get(int articleId);

        IEnumerable<IArticle> GetList(int orderby, int articleType = 0);

        void Delete(int articleId);

        void Update(IArticle article);

        void Add(IArticle article);

        void Publish(IArticle article);
    }
}