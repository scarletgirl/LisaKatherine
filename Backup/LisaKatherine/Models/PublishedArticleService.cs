using System;
using System.Collections.Generic;
using System.Linq;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Models
{
    public class PublishedArticleService
    {

        private readonly LisaKatherineEntities _dataModel;
        private readonly UserService _userService;

        public PublishedArticleService(LisaKatherineEntities dataModel, UserService userService)
        {
            _dataModel = dataModel;
            _userService = userService;
        }

        public PublishedArticleService()
        {
            _dataModel = new LisaKatherineEntities();
            _userService = new UserService();
        }

        public PublishedArticles GetPublishedArticle(int id)
        {
            var article = (from a in _dataModel.PublishedArticles where a.articleId == id select a).First();
            ExtendPublishedArticle(article);
            return article;
        }

        private PublishedArticles ExtendPublishedArticle(PublishedArticles article)
        {
            article.user = (from u in _userService.GetList() where u.userId == article.userid select u).First();
            article.articleType = (from at in new ArticleTypeService().GetArticleTypesList() where at.articleTypeId == article.articleTypeId select at).First();
            article.Description = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            article.Url = string.Format("{0}/{1}", Utils.StripInvalid(article.headline), article.articleId);
            return article;
        }

        public PublishedArticles GetArticleByArticleType(int articleTypeId)
        {
            PublishedArticles article = null;
            var articles = (from a in _dataModel.PublishedArticles where a.articleTypeId == articleTypeId orderby a.datePublished descending select a);
            if (articles.Count() > 0)
            {
                article = articles.First();
                ExtendPublishedArticle(article);

            }
            return article;
        }

        public PublishedArticles GetArticleLatestBlog(int section)
        {
            var listIds = new List<int>();
            listIds.Add(section);
            return GetPublishedArticlesManyTypes(listIds).OrderByDescending(a => a.datePublished).First();
        }


        public IEnumerable<PublishedArticles> GetPublishedList(int articleTypeId)
        {
            var users = _userService.GetList();
            var articleTypes = new ArticleTypeService().GetArticleTypesList();
            var articleList = (from a in _dataModel.PublishedArticles where a.articleTypeId == articleTypeId orderby a.datePublished descending select a);
            foreach (PublishedArticles article in articleList)
            {
                //article.user = (from u in users where u.userId == article.userid select u).First();
                //article.articleType = (from at in articleTypes where at.articleTypeId == article.articleTypeId select at).First();
                ExtendPublishedArticle(article);
                article.body = Utils.GetSummary(Utils.StripHtml(article.body), 255);
            }
            return articleList;
        }

        public IEnumerable<PublishedArticles> GetPublishedArticlesManyTypes(List<int> articleTypes)
        {
            var articles = new List<PublishedArticles>();
            articleTypes.ForEach(i => articles.AddRange(GetPublishedList(i)));
            return articles;
        }

        public PagedList<PublishedArticles> GetBlogList(int id, int? page) {
            IEnumerable<PublishedArticles> articles = GetPublishedList(id);
            PagedList<PublishedArticles> pa = new PagedList<PublishedArticles>(articles, page ?? 1, 5, articles.Count());
            return pa;
        }
    }
}