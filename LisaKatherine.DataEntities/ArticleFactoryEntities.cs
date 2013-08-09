namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LisaKatherine.Interface;

    public class ArticleFactoryEntities : IArticleFactory
    {
        private readonly LisaKatherineEntities dataModel;

        private readonly IUserFactory userFactory;

        private readonly IArticleTypeFactory articleTypeFactory;

        public ArticleFactoryEntities()
        {
            this.dataModel = new LisaKatherineEntities();
            this.userFactory = new UserFactoryEntities();
            this.articleTypeFactory = new ArticleTypeFactoryEntities();
        }

        public ArticleFactoryEntities(
            LisaKatherineEntities dataModel, IUserFactory userFactory, IArticleTypeFactory articleTypeFactory)
        {
            this.dataModel = dataModel;
            this.articleTypeFactory = articleTypeFactory;
            this.userFactory = userFactory;
        }

        public IArticle Get(int articleId)
        {
            Articles x = (from a in this.dataModel.Articles1 where a.articleId == articleId select a).First();
            IUser user = null;
            if (x.userid != null)
            {
                user = this.userFactory.Get((Guid)x.userid);
            }

            IArticleType articleType = null;
            if (x.articleTypeId != null)
            {
                articleType = this.articleTypeFactory.Get((int)x.articleTypeId);
            }

            return new Article(
                x.headline,
                x.strapline,
                x.body,
                x.dateCreated,
                x.datePublished,
                x.isPublished,
                x.articleTypeId,
                user,
                articleType) { ArticleId = articleId };
        }

        public IEnumerable<IArticle> GetList(int orderby)
        {
            IOrderedQueryable<Articles> articleList = this.dataModel.Articles1.OrderBy(a => a.headline);
            return articleList.Select(a => this.Get(a.articleId)).ToList();
        }

        public void Delete(int articleId)
        {
            throw new NotImplementedException();
        }

        public void Update(IArticle article)
        {
            Articles originalArticle =
                (from a in this.dataModel.Articles1 where a.articleId == article.ArticleId select a).First();

            if (originalArticle.datePublished.HasValue)
            {
                article.DatePublished = originalArticle.datePublished;
            }
            article.DateCreated = originalArticle.dateCreated;

            this.dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, article);
            this.dataModel.SaveChanges();
        }

        public void Add(IArticle article)
        {
            article.DateCreated = DateTime.Now;
            if (article.User != null)
            {
                var a = new Articles
                            {
                                articleId = article.ArticleId,
                                articleTypeId = article.ArticleTypeId,
                                body = article.Body,
                                dateCreated = DateTime.Now,
                                headline = article.Headline,
                                isPublished = article.IsPublished,
                                strapline = article.Strapline,
                                userid = article.User.UserId
                            };

                this.dataModel.AddToArticles1(a);
                this.dataModel.SaveChanges();
            }
        }

        public void Public(IArticle article)
        {
            var articleToPublish = new PublishedArticles
                                       {
                                           articleId = article.ArticleId,
                                           articleTypeId = article.ArticleTypeId,
                                           body = article.Body,
                                           headline = article.Headline,
                                           strapline = article.Strapline,
                                           userid = article.Userid,
                                           datePublished = article.DatePublished,
                                           isPublished = true
                                       };

            IQueryable<PublishedArticles> originalPublishedArticle =
                (from pa in this.dataModel.PublishedArticles where pa.articleId == article.ArticleId select pa);

            if (originalPublishedArticle.Any())
            {
                this.dataModel.ApplyCurrentValues(
                    originalPublishedArticle.First().EntityKey.EntitySetName, articleToPublish);
            }
            else
            {
                this.dataModel.AddToPublishedArticles(articleToPublish);
            }

            this.dataModel.SaveChanges();
        }
    }
}