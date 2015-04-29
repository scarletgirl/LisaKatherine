﻿namespace LisaKatherine.Services
{
    using LisaKatherine.DataEntitiesRepository;

    public class FacebookComment
    {
        public FacebookComment(FacebookUser fbUser, FacebookPost fbPost)
        {
            this.FbUser = fbUser;
            this.FbPost = fbPost;
        }

        public FacebookUser FbUser { get; set; }

        public FacebookPost FbPost { get; set; }
    }
}