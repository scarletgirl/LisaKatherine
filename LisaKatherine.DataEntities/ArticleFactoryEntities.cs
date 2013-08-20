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

        public ArticleFactoryEntities(LisaKatherineEntities dataModel, IUserFactory userFactory, IArticleTypeFactory articleTypeFactory)
        {
            this.dataModel = dataModel;
            this.articleTypeFactory = articleTypeFactory;
            this.userFactory = userFactory;
        }

        public IArticle Get(int articleId)
        {
            ArticleEntity x = (from a in this.dataModel.Articles1 where a.articleId == articleId select a).First();
            return this.SetArticle(articleId, x);
        }

        public IEnumerable<IArticle> GetList(int orderby, int articleTypeId = 0)
        {
            IOrderedQueryable<ArticleEntity> articleList = this.dataModel.Articles1.Where(a => a.articleTypeId == articleTypeId || articleTypeId == 0).OrderBy(a => a.headline);
            var list = new List<IArticle>();
            foreach (ArticleEntity a in articleList)
            {
                list.Add(this.SetArticle(a.articleId, a));
            }
            return list;
        }

        public void Delete(int articleId)
        {
            ArticleEntity articleEntity = (from a in this.dataModel.Articles1 where a.articleId == articleId select a).First();

            this.dataModel.DeleteObject(articleEntity);
            this.dataModel.SaveChanges();
        }

        public void Update(IArticle article)
        {
            ArticleEntity originalArticle = (from a in this.dataModel.Articles1 where a.articleId == article.ArticleId select a).First();

            this.dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, ConvertArticleEntity(article, originalArticle));
            this.dataModel.SaveChanges();
        }

        public void Add(IArticle article)
        {
            article.DateCreated = DateTime.Now;
            if (article.User != null)
            {
                this.dataModel.AddToArticles1(ConvertArticleEntity(article));
                this.dataModel.SaveChanges();
            }
        }

        public void Publish(IArticle article)
        {
            IQueryable<PublishedArticleEntity> originalPublishedArticle = (from pa in this.dataModel.PublishedArticles where pa.articleId == article.ArticleId select pa);

            if (originalPublishedArticle.Any())
            {
                this.dataModel.ApplyCurrentValues(originalPublishedArticle.First().EntityKey.EntitySetName, ConvertArticleEntity(article));
            }
            else
            {
                this.dataModel.AddToPublishedArticles(PublishArticleFactoryEntities.ConvertPublishArticleEntity(article));
            }

            this.dataModel.SaveChanges();
        }

        private static ArticleEntity ConvertArticleEntity(IArticle article, ArticleEntity originalArticle = null)
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

            return new ArticleEntity
                       {
                           articleTypeId = article.ArticleTypeId,
                           articleId = article.ArticleId,
                           body = article.Body,
                           headline = article.Headline,
                           isPublished = article.IsPublished,
                           dateCreated = dc,
                           datePublished = dp,
                           strapline = article.Strapline,
                           userid = article.User.UserId,
                       };
        }

        private IArticle SetArticle(int articleId, ArticleEntity x)
        {
            IUser user = new User();
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