namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface IArticleTypeFactory
    {
        IArticleType Get(int articleTypeId);

        IEnumerable<IArticleType> GetList();

        bool Delete(int articleTypeId);

        void Update(IArticleType articleType);

        void Add(IArticleType articleType);
    }
}