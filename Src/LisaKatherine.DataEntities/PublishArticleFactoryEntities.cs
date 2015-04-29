using System;
using System.Collections.Generic;
using System.Linq;
using LisaKatherine.Interface;

namespace LisaKatherine.DataEntitiesRepository
{
    public class PublishArticleFactoryEntities : IArticleFactory
    {
        private readonly IArticleTypeFactory articleTypeFactory;

        private readonly LisaKatherineEntities dataModel;

        private readonly IUserFactory userFactory;

        public PublishArticleFactoryEntities()
        {
            dataModel = new LisaKatherineEntities();
            userFactory = new UserFactoryEntities();
            articleTypeFactory = new ArticleTypeFactoryEntities();
        }

        public PublishArticleFactoryEntities(LisaKatherineEntities dataModel, IUserFactory userFactory, IArticleTypeFactory articleTypeFactory)
        {
            this.dataModel = dataModel;
            this.articleTypeFactory = articleTypeFactory;
            this.userFactory = userFactory;
        }

        public static PublishedArticleEntity ConvertPublishArticleEntity(IArticle article, PublishedArticleEntity originalArticle = null)
        {
            var dc = article.DateCreated;
            if (dc == null)
            {
                dc = DateTime.Now;
            }

            var dp = article.DatePublished;
            if (dp == null)
            {
                dp = DateTime.Now;
            }

            return new PublishedArticleEntity
            {
                articleTypeId = article.ArticleTypeId,
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

        public IArticle Get(int articleId)
        {
            var articleEntity =
                (from a in dataModel.PublishedArticleEntity where a.articleId == articleId select a).First();
            return SetArticle(articleId, articleEntity);
        }

        public IEnumerable<IArticle> GetList(int orderby, int articleTypeId = 0)
        {
            var articleList =
                dataModel.PublishedArticleEntity.Where(a => a.articleTypeId == articleTypeId || articleTypeId == 0)
                    .OrderByDescending(a => a.datePublished);

            return articleList.Select(a => SetArticle(a.articleId, a)).ToList();
        }

        public void Delete(int articleId)
        {
            var articleEntity =
                (from a in dataModel.PublishedArticleEntity where a.articleId == articleId select a).First();
            dataModel.DeleteObject(articleEntity);
            dataModel.SaveChanges();
        }

        public void Update(IArticle article)
        {
            var originalArticle =
                (from a in dataModel.PublishedArticleEntity where a.articleId == article.ArticleId select a).First();

            dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, ConvertPublishArticleEntity(article, originalArticle));
            dataModel.SaveChanges();
        }

        public void Add(IArticle article)
        {
            article.DateCreated = DateTime.Now;
            if (article.User != null)
            {
                dataModel.AddToPublishedArticleEntity(ConvertPublishArticleEntity(article));
                dataModel.SaveChanges();
            }
        }

        public void Publish(IArticle article)
        {
            var originalPublishedArticle = from pa in dataModel.PublishedArticleEntity where pa.articleId == article.ArticleId select pa;

            if (originalPublishedArticle.Any())
            {
                dataModel.ApplyCurrentValues(originalPublishedArticle.First().EntityKey.EntitySetName, ConvertPublishArticleEntity(article));
            }
            else
            {
                dataModel.AddToPublishedArticleEntity(ConvertPublishArticleEntity(article));
            }

            dataModel.SaveChanges();
        }

        private IArticle SetArticle(int articleId, PublishedArticleEntity x)
        {
            IUser user = null;
            if (x.userid != null)
            {
                user = userFactory.Get((Guid)x.userid);
            }

            IArticleType articleType = new ArticleType();
            if (x.articleTypeId != null)
            {
                try
                {
                    articleType = articleTypeFactory.Get((int)x.articleTypeId);
                }
                catch (Exception)
                {
                    throw new Exception("Cannot find " + x.articleTypeId);
                }
            }

            return new Article(x.headline, x.strapline, x.body, x.dateCreated, x.datePublished, x.isPublished, x.articleTypeId, user, articleType) { ArticleId = articleId };
        }
    }
}