namespace LisaKatherine.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Webdiyer.WebControls.Mvc;

    public class PublishedArticleService
    {
        private readonly LisaKatherineEntities dataModel;

        private readonly UserService userService;

        public PublishedArticleService(LisaKatherineEntities dataModel, UserService userService)
        {
            this.dataModel = dataModel;
            this.userService = userService;
        }

        public PublishedArticleService()
        {
            this.dataModel = new LisaKatherineEntities();
            this.userService = new UserService();
        }

        public PublishedArticles GetPublishedArticle(int id)
        {
            PublishedArticles article =
                (from a in this.dataModel.PublishedArticles where a.articleId == id select a).First();
            this.ExtendPublishedArticle(article);
            return article;
        }

        public PublishedArticles GetArticleByArticleType(int articleTypeId)
        {
            PublishedArticles article = null;
            IOrderedQueryable<PublishedArticles> articles = (from a in this.dataModel.PublishedArticles
                                                             where a.articleTypeId == articleTypeId
                                                             orderby a.datePublished descending
                                                             select a);
            if (articles.Any())
            {
                article = articles.First();
                this.ExtendPublishedArticle(article);
            }
            return article;
        }

        public PublishedArticles GetArticleLatestBlog(int section)
        {
            var listIds = new List<int> { section };
            return this.GetPublishedArticlesManyTypes(listIds).OrderByDescending(a => a.datePublished).First();
        }

        public IEnumerable<PublishedArticles> GetPublishedList(int articleTypeId)
        {
            IOrderedQueryable<PublishedArticles> articleList = (from a in this.dataModel.PublishedArticles
                                                                where a.articleTypeId == articleTypeId
                                                                orderby a.datePublished descending
                                                                select a);
            foreach (PublishedArticles article in articleList)
            {
                this.ExtendPublishedArticle(article);
                article.body = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            }
            return articleList;
        }

        public IEnumerable<PublishedArticles> GetPublishedArticlesManyTypes(List<int> articleTypes)
        {
            var articles = new List<PublishedArticles>();
            articleTypes.ForEach(i => articles.AddRange(this.GetPublishedList(i)));
            return articles;
        }

        public PagedList<PublishedArticles> GetBlogList(int id, int? page)
        {
            IEnumerable<PublishedArticles> articles = this.GetPublishedList(id);

            if (articles != null)
            {
                var pa = new PagedList<PublishedArticles>(articles, page ?? 1, 5, articles.Count());
                return pa;
            }
            return null;
        }

        private void ExtendPublishedArticle(PublishedArticles article)
        {
            article.user = (from u in this.userService.GetList() where u.userId == article.userid select u).First();
            article.articleType =
                (from at in new ArticleTypeService().GetArticleTypesList()
                 where at.articleTypeId == article.articleTypeId
                 select at).First();
            article.Description = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            article.Url = string.Format("{0}/{1}", Utils.StripInvalid(article.headline), article.articleId);
        }
    }
}