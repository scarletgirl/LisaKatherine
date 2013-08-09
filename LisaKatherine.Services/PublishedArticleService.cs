namespace LisaKatherine.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using LisaKatherine.DataEntitiesRepository;
    using LisaKatherine.Interface;

    using Webdiyer.WebControls.Mvc;

    public class PublishedArticleService
    {
        private readonly IArticleFactory publishedArticleFactory = new PublishArticleFactoryEntities();

        private readonly UserService userService;

        public PublishedArticleService(IArticleFactory dataModel, UserService userService)
        {
            this.publishedArticleFactory = dataModel;
            this.userService = userService;
        }

        public PublishedArticleService()
        {
            this.publishedArticleFactory = new PublishArticleFactoryEntities();
            this.userService = new UserService();
        }

        public IPublishedArticle GetPublishedArticle(int id)
        {
            IArticle article =
                (from a in this.publishedArticleFactory.GetList(0) where a.ArticleId == id select a).First();

            return this.ExtendPublishedArticle(article);
        }

        public IPublishedArticle GetArticleByArticleType(int articleTypeId)
        {
            IArticle article = null;
            IEnumerable<IArticle> articles = (from a in this.publishedArticleFactory.GetList(0)
                                                             where a.ArticleTypeId == articleTypeId
                                                             orderby a.DatePublished descending
                                                             select a);
            if (articles.Any())
            {
                article = articles.First();
                return this.ExtendPublishedArticle(article);
            }
            return null;
        }

        public IPublishedArticle GetArticleLatestBlog(int section)
        {
            var listIds = new List<int> { section };
            return this.GetPublishedArticlesManyTypes(listIds).OrderByDescending(a => a.DatePublished).First();
        }

        public IEnumerable<IPublishedArticle> GetPublishedList(int articleTypeId)
        {
            var articleList = new List<IPublishedArticle>();
            foreach (var article in publishedArticleFactory.GetList(0))
            {
                article.Body = Utils.GetSummary(Utils.StripHtml(article.Body), 255);
                articleList.Add(this.ExtendPublishedArticle(article));
            }
            return articleList;
        }

        public IEnumerable<IPublishedArticle> GetPublishedArticlesManyTypes(List<int> articleTypes)
        {
            var articles = new List<IPublishedArticle>();
            articleTypes.ForEach(i => articles.AddRange(this.GetPublishedList(i)));
            return articles;
        }

        public PagedList<IPublishedArticle> GetBlogList(int id, int? page)
        {
            IEnumerable<IPublishedArticle> articles = this.GetPublishedList(id);

            if (articles != null)
            {
                var pa = new PagedList<IPublishedArticle>(articles, page ?? 1, 5, articles.Count());
                return pa;
            }
            return null;
        }

        private IPublishedArticle ExtendPublishedArticle(IArticle article)
        {
            article.User = (from u in this.userService.GetList() where u.UserId == article.Userid select u).First();
            article.ArticleType =
                (from at in new ArticleTypeService().GetArticleTypesList()
                 where at.ArticleTypeId == article.ArticleTypeId
                 select at).First();

            var publishedArticle = new PublishedArticle(
                article.Headline,
                article.Strapline,
                article.Body,
                article.DateCreated,
                article.DatePublished,
                article.IsPublished,
                article.ArticleTypeId,
                article.User,
                article.ArticleType)
                                       {
                                           Description = Utils.GetSummary(Utils.StripHtml(article.Body), 255),
                                           Url =
                                               string.Format(
                                                   "{0}/{1}", Utils.StripInvalid(article.Headline), article.ArticleId)
                                       };

            return publishedArticle;
        }
    }
}