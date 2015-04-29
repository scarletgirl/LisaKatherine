using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LisaKatherine.Models
{
    public class FacebookComment
    {
        public FacebookUser FBuser { get; set; }
        public FacebookPost FBPost { get; set; }

        public FacebookComment(FacebookUser fbUser, FacebookPost fbPost)
        {
            FBuser = fbUser;
            FBPost = fbPost;
        }

    }
}