namespace LisaKatherine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Webdiyer.WebControls.Mvc;

    public class FacebookService
    {
        private readonly LisaKatherineEntities dataModel;

        public FacebookService(LisaKatherineEntities dataModel)
        {
            this.dataModel = dataModel;
        }

        public FacebookService()
        {
            this.dataModel = new LisaKatherineEntities();
        }

        public void AddFacebookComment(FacebookUser fbUser, string comment, PublishedArticles article)
        {
            var fbPost = new FacebookPost
                             {
                                 articleId = article.articleId,
                                 facebookId = fbUser.facebookId,
                                 post = comment
                             };

            this.dataModel.AddToFacebookPosts(fbPost);
        }

        public IEnumerable<FacebookComment> GetCommentsForArticle(int id)
        {
            IOrderedQueryable<FacebookPost> posts =
                this.dataModel.FacebookPosts.Where(a => a.articleId == id).OrderBy(a => a.dateAdded);
            List<FacebookComment> list =
                (from post in posts
                 let fbUser = this.GetFacebookUser(post.facebookId)
                 select new FacebookComment(fbUser, post)).ToList();
            return new PagedList<FacebookComment>(list, 1, 20);
        }

        public FacebookUser GetFacebookUser(long userId)
        {
            return this.dataModel.FacebookUsers.First(fbu => fbu.facebookId == userId);
        }

        public FacebookUser AddFacebookUser(long facebookId, string accessToken, string name, string gender)
        {
            IQueryable<FacebookUser> fbUsers = this.dataModel.FacebookUsers.Where(f => f.facebookId == facebookId);
            if (!fbUsers.Any())
            {
                //add fbuser to db
                var fbu = new FacebookUser
                              {
                                  facebookId = facebookId,
                                  accessToken = accessToken,
                                  gender = gender,
                                  name = name,
                                  firstlogin = DateTime.Now,
                                  lastlogin = DateTime.Now
                              };
                this.dataModel.AddToFacebookUsers(fbu);
                this.dataModel.SaveChanges();
                return fbu;
            }

            return fbUsers.First();
        }
    }
}