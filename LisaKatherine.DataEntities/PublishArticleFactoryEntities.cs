namespace LisaKatherine.DataEntitiesRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LisaKatherine.Interface;

    public class PublishArticleFactoryEntities : IArticleFactory
    {
        private readonly LisaKatherineEntities dataModel;

        private readonly IUserFactory userFactory;

        private readonly IArticleTypeFactory articleTypeFactory;

        public PublishArticleFactoryEntities()
        {
            this.dataModel = new LisaKatherineEntities();
            this.userFactory = new UserFactoryEntities();
            this.articleTypeFactory = new ArticleTypeFactoryEntities();
        }

        public PublishArticleFactoryEntities(LisaKatherineEntities dataModel, IUserFactory userFactory, IArticleTypeFactory articleTypeFactory)
        {
            this.dataModel = dataModel;
            this.articleTypeFactory = articleTypeFactory;
            this.userFactory = userFactory;
        }

        public IArticle Get(int articleId)
        {
            PublishedArticleEntity articleEntity = (from a in this.dataModel.PublishedArticles where a.articleId == articleId select a).First();
            return this.SetArticle(articleId, articleEntity);
        }

        public IEnumerable<IArticle> GetList(int orderby, int articleTypeId = 0)
        {
            IOrderedQueryable<PublishedArticleEntity> articleList =
                this.dataModel.PublishedArticles.Where(a => a.articleTypeId == articleTypeId || articleTypeId == 0).OrderByDescending(a => a.datePublished);
            var list = new List<IArticle>();

            foreach (PublishedArticleEntity a in articleList)
            {
                list.Add(this.SetArticle(a.articleId, a));
            }
            return list;
        }

        public void Delete(int articleId)
        {
            PublishedArticleEntity articleEntity = (from a in this.dataModel.PublishedArticles where a.articleId == articleId select a).First();
            this.dataModel.DeleteObject(articleEntity);
            this.dataModel.SaveChanges();
        }

        public void Update(IArticle article)
        {
            PublishedArticleEntity originalArticle = (from a in this.dataModel.PublishedArticles where a.articleId == article.ArticleId select a).First();

            this.dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, ConvertPublishArticleEntity(article, originalArticle));
            this.dataModel.SaveChanges();
        }

        public void Add(IArticle article)
        {
            article.DateCreated = DateTime.Now;
            if (article.User != null)
            {
                this.dataModel.AddToPublishedArticles(ConvertPublishArticleEntity(article));
                this.dataModel.SaveChanges();
            }
        }

        public void Publish(IArticle article)
        {
            IQueryable<PublishedArticleEntity> originalPublishedArticle = (from pa in this.dataModel.PublishedArticles where pa.articleId == article.ArticleId select pa);

            if (originalPublishedArticle.Any())
            {
                this.dataModel.ApplyCurrentValues(originalPublishedArticle.First().EntityKey.EntitySetName, ConvertPublishArticleEntity(article));
            }
            else
            {
                this.dataModel.AddToPublishedArticles(ConvertPublishArticleEntity(article));
            }

            this.dataModel.SaveChanges();
        }

        public static PublishedArticleEntity ConvertPublishArticleEntity(IArticle article, PublishedArticleEntity originalArticle = null)
        {
            DateTime dc = article.DateCreated;
            if (originalArticle != null)
            {
                dc = originalArticle.dateCreated;
            }

            DateTime? dp = article.DatePublished;
            if (originalArticle != null)
            {
                dp = originalArticle.datePublished;
            }

            return new PublishedArticleEntity
                       {
                           articleTypeId = article.ArticleId,
                           articleId = article.ArticleId,
                           body = article.Body,
                           headline = article.Headline,
                           isPublished = article.IsPublished,
                           dateCreated = dc,
                           datePublished = dp,
                           strapline = article.Strapline,
                           userid = article.Userid
                       };
        }

        private IArticle SetArticle(int articleId, PublishedArticleEntity x)
        {
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

            return new Article(x.headline, x.strapline, x.body, x.dateCreated, x.datePublished, x.isPublished, x.articleTypeId, user, articleType) { ArticleId = articleId };
        }
    }
}