using System;
using System.Collections.Generic;
using System.Linq;

namespace LisaKatherine.Models
{
    public class ArticleService
    {

        private readonly LisaKatherineEntities _dataModel;
        private readonly UserService _userService;

        public ArticleService(LisaKatherineEntities dataModel, UserService userService)
        {
            _dataModel = dataModel;
            _userService = userService;
        }

        public ArticleService()
        {
            _dataModel = new LisaKatherineEntities();
            _userService = new UserService();
        }

        public void EditArticle(Articles article)
        {
            var originalArticle = (from a in _dataModel.Articles1 where a.articleId == article.articleId select a).First();
            if (originalArticle.datePublished.HasValue)
            {
                article.datePublished = originalArticle.datePublished;
            }
            article.dateCreated = originalArticle.dateCreated;

            _dataModel.ApplyCurrentValues(originalArticle.EntityKey.EntitySetName, article);
            _dataModel.SaveChanges();
        }

        public IEnumerable<Articles> GetList(int orderby)
        {
            var users = _userService.GetList();
            var articleTypes = new ArticleTypeService().GetArticleTypesList();
            var articleList = _dataModel.Articles1.OrderBy(a => a.headline);
            foreach (Articles article in articleList)
            {
                article.user = (from u in users where u.userId == article.userid select u).First();
                article.articleType = (from at in articleTypes where at.articleTypeId == article.articleTypeId select at).First();
            }
            return articleList;
        }

        public void CreateArticle(Articles article)
        {

            article.dateCreated = DateTime.Now;
            Users user = _userService.CheckSession();
            if (user != null)
            {
                article.userid = user.userId;
                _dataModel.AddToArticles1(article);
                _dataModel.SaveChanges();
            }
        }

        public Articles GetArticle(int id)
        {
            var article = (from a in _dataModel.Articles1 where a.articleId == id select a).First();
            ExtendArticle(article);
            return article;
        }

        public void DeleteArticle(int id)
        {
            var article = (from a in _dataModel.Articles1 where a.articleId == id select a).First();
            _dataModel.DeleteObject(article);
            _dataModel.SaveChanges();
        }

        public Articles ExtendArticle(Articles article)
        {
            article.user = (from u in _userService.GetList() where u.userId == article.userid select u).First();
            article.articleType = (from at in new ArticleTypeService().GetArticleTypesList() where at.articleTypeId == article.articleTypeId select at).First();
            return article;
        }


        /**** Published Articles ****/

        public void PublishArticle(Articles article)
        {
            PublishedArticles articleToPublish = new PublishedArticles()
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
            var originalPublishedArticle = (from pa in _dataModel.PublishedArticles where pa.articleId == article.articleId select pa);
            if (originalPublishedArticle.Count() > 0)
            {
                _dataModel.ApplyCurrentValues(originalPublishedArticle.First().EntityKey.EntitySetName, articleToPublish);
            }
            else
            {
                _dataModel.AddToPublishedArticles(articleToPublish);

            }
            _dataModel.SaveChanges();

        }


    }
}