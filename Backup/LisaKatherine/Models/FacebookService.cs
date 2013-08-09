using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace LisaKatherine.Models
{
    public class FacebookService
    {
        private readonly LisaKatherineEntities _dataModel;


        public FacebookService(LisaKatherineEntities dataModel)
        {
            _dataModel = dataModel;

        }

        public FacebookService()
        {
            _dataModel = new LisaKatherineEntities();
        }

        public void AddFacebookComment(FacebookUser fbUser, string comment, PublishedArticles article)
        {
            var fbPost = new FacebookPost();
            fbPost.articleId = article.articleId;
            fbPost.facebookId = fbUser.facebookId;
            fbPost.post = comment;

            _dataModel.AddToFacebookPosts(fbPost);
        }

        public IEnumerable<FacebookComment> GetCommentsForArticle(int id)
        {
            var list = new List<FacebookComment>();
            var posts = _dataModel.FacebookPosts.Where(a => a.articleId == id).OrderBy(a => a.dateAdded);
            foreach (var post in posts) { 
                var fbUser = GetFacebookUser(post.facebookId);
                list.Add(new FacebookComment(fbUser, post));
            }
            return new PagedList<FacebookComment>(list, 1, 20);
        }


        public FacebookUser GetFacebookUser(long userId)
        {
            return _dataModel.FacebookUsers.Where(fbu => fbu.facebookId == userId).First();
        }

        public FacebookUser AddFacebookUser(long facebookId, string accessToken, string name, string gender)
        {
            var fbUsers = _dataModel.FacebookUsers.Where(f => f.facebookId == facebookId);
            if (fbUsers.Count() == 0)
            {
                //add fbuser to db
                var fbu = new FacebookUser();
                fbu.facebookId = facebookId;
                fbu.accessToken = accessToken;
                fbu.gender = gender;
                fbu.name = name;
                fbu.firstlogin = DateTime.Now;
                fbu.lastlogin = DateTime.Now;
                _dataModel.AddToFacebookUsers(fbu);
                _dataModel.SaveChanges();
                return fbu;
            }
            else
            { 
                //update last login date
                return fbUsers.First();
            }
        }

    }
}