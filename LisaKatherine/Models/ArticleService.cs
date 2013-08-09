namespace LisaKatherine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ArticleService
    {
        private readonly LisaKatherineEntities dataModel;

        private readonly UserService userService;

        public ArticleService(LisaKatherineEntities dataModel, UserService userService)
        {
            this.dataModel = dataModel;
            this.userService = userService;
        }

        public ArticleService()
        {
            this.dataModel = new LisaKatherineEntities();
            this.userService = new UserService();
        }

        public void EditArticle(Articles article)
        {
            Articles originalArticle =
                (from a in this.dataModel.Articles1 where a.articleId == article.articleId select a).First();
            if (originalArticle.datePublished.HasValue)
            {
                article.datePublished = originalArticle.datePublished;
            }
            article.dateCreated = originalArticle.dateCreated;

            this.dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, article);
            this.dataModel.SaveChanges();
        }

        public IEnumerable<Articles> GetList(int orderby)
        {
            IEnumerable<Users> users = this.userService.GetList();
            IEnumerable<ArticleTypes> articleTypes = new ArticleTypeService().GetArticleTypesList();
            IOrderedQueryable<Articles> articleList = this.dataModel.Articles1.OrderBy(a => a.headline);
            foreach (Articles article in articleList)
            {
                article.user = (from u in users where u.userId == article.userid select u).First();
                article.articleType =
                    (from at in articleTypes where at.articleTypeId == article.articleTypeId select at).First();
            }
            return articleList;
        }

        public void CreateArticle(Articles article)
        {
            article.dateCreated = DateTime.Now;
            Users user = this.userService.CheckSession();
            if (user != null)
            {
                article.userid = user.userId;
                this.dataModel.AddToArticles1(article);
                this.dataModel.SaveChanges();
            }
        }

        public Articles GetArticle(int id)
        {
            Articles article = (from a in this.dataModel.Articles1 where a.articleId == id select a).First();
            this.ExtendArticle(article);
            return article;
        }

        public void DeleteArticle(int id)
        {
            Articles article = (from a in this.dataModel.Articles1 where a.articleId == id select a).First();
            this.dataModel.DeleteObject(article);
            this.dataModel.SaveChanges();
        }

        public Articles ExtendArticle(Articles article)
        {
            article.user = (from u in this.userService.GetList() where u.userId == article.userid select u).First();
            article.articleType =
                (from at in new ArticleTypeService().GetArticleTypesList()
                 where at.articleTypeId == article.articleTypeId
                 select at).First();
            return article;
        }

        /**** Published Articles ****/

        public void PublishArticle(Articles article)
        {
            var articleToPublish = new PublishedArticles
                                       {
                                           articleId = article.articleId,
                                           articleTypeId = article.articleTypeId,
                                           body = article.body,
                                           headline = article.headline,
                                           strapline = article.strapline,
                                           userid = article.userid,
                                           datePublished = article.datePublished,
                                           isPublished = true
                                       };

            IQueryable<PublishedArticles> originalPublishedArticle =
                (from pa in this.dataModel.PublishedArticles where pa.articleId == article.articleId select pa);

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