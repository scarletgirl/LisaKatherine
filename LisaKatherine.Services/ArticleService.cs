namespace LisaKatherine.Services
{
    using System.Collections.Generic;

    using LisaKatherine.DataEntitiesRepository;
    using LisaKatherine.Interface;
    using LisaKatherine.Models;

    public class ArticleService
    {
        private readonly IArticleFactory articleFactory;

        private readonly UserService userService;

        public ArticleService(IArticleFactory articleFactory, UserService userService)
        {
            this.articleFactory = articleFactory;
            this.userService = userService;
        }

        public ArticleService()
        {
            this.articleFactory = new ArticleFactoryEntities();
            this.userService = new UserService();
        }

        public void EditArticle(IArticle article)
        {
            this.articleFactory.Update(article);
        }

        public IEnumerable<IArticle> GetList(int orderby)
        {
            return this.articleFactory.GetList(orderby);
        }

        public void CreateArticle(IArticle article)
        {
            IUser user = this.userService.CheckSession();
            if (user != null)
            {
                article.User = user;
                this.articleFactory.Add(article);
            }
        }

        public IArticle GetArticle(int id)
        {
            return this.articleFactory.Get(id);
        }

        public void DeleteArticle(int id)
        {
            this.articleFactory.Delete(id);
        }

        /**** Published Articles ****/

        public void PublishArticle(IArticle article)
        {
            this.articleFactory.Public(article);
        }
    }
}