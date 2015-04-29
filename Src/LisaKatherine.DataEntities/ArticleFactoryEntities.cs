using System;
using System.Collections.Generic;
using System.Linq;
using LisaKatherine.Interface;

namespace LisaKatherine.DataEntitiesRepository
{
    public class ArticleFactoryEntities : IArticleFactory
    {
        private readonly IArticleTypeFactory articleTypeFactory;
        private readonly LisaKatherineEntities dataModel;
        private readonly IUserFactory userFactory;

        public ArticleFactoryEntities()
        {
            dataModel = new LisaKatherineEntities();
            userFactory = new UserFactoryEntities();
            articleTypeFactory = new ArticleTypeFactoryEntities();
        }

        public ArticleFactoryEntities(LisaKatherineEntities dataModel, IUserFactory userFactory, IArticleTypeFactory articleTypeFactory)
        {
            this.dataModel = dataModel;
            this.articleTypeFactory = articleTypeFactory;
            this.userFactory = userFactory;
        }

        public IArticle Get(int articleId)
        {
            var x = (from a in dataModel.ArticleEntity where a.articleId == articleId select a).First();
            return SetArticle(articleId, x);
        }

        public IEnumerable<IArticle> GetList(int orderby, int articleTypeId = 0)
        {
            var articleList = dataModel.ArticleEntity.Where(a => a.articleTypeId == articleTypeId || articleTypeId == 0).OrderBy(a => a.headline);
            return articleList.Select(a => SetArticle(a.articleId, a)).ToList();
        }

        public void Delete(int articleId)
        {
            var articleEntity = (from a in dataModel.ArticleEntity where a.articleId == articleId select a).First();

            dataModel.DeleteObject(articleEntity);
            dataModel.SaveChanges();
        }

        public void Update(IArticle article)
        {
            var originalArticle = (from a in dataModel.ArticleEntity where a.articleId == article.ArticleId select a).First();

            dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, ConvertArticleEntity(article, originalArticle));
            dataModel.SaveChanges();
        }

        public void Add(IArticle article)
        {
            article.DateCreated = DateTime.Now;
            if (article.User != null)
            {
                dataModel.AddToArticleEntity(ConvertArticleEntity(article));
                dataModel.SaveChanges();
            }
        }

        public void Publish(IArticle article)
        {
            var originalPublishedArticle = from pa in dataModel.PublishedArticleEntity where pa.articleId == article.ArticleId select pa;

            if (originalPublishedArticle.Any())
            {
                dataModel.ApplyCurrentValues(originalPublishedArticle.First().EntityKey.EntitySetName, PublishArticleFactoryEntities.ConvertPublishArticleEntity(article));
            }
            else
            {
                dataModel.AddToPublishedArticleEntity(PublishArticleFactoryEntities.ConvertPublishArticleEntity(article));
            }

            dataModel.SaveChanges();
        }

        private static ArticleEntity ConvertArticleEntity(IArticle article, ArticleEntity originalArticle = null)
        {
            var dc = article.DateCreated;
            if (originalArticle != null)
            {
                dc = originalArticle.dateCreated;
            }

            var dp = article.DatePublished;
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
                userid = article.User.UserId
            };
        }

        private IArticle SetArticle(int articleId, ArticleEntity x)
        {
            IUser user = new User();
            if (x.userid != null)
            {
                user = userFactory.Get((Guid)x.userid);
            }

            IArticleType articleType = null;
            if (x.articleTypeId != null)
            {
                articleType = articleTypeFactory.Get((int)x.articleTypeId);
            }

            return new Article(x.headline, x.strapline, x.body, x.dateCreated, x.datePublished, x.isPublished, x.articleTypeId, user, articleType) { ArticleId = articleId };
        }
    }
}