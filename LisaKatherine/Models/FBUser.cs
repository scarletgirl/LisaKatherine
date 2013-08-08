using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LisaKatherine.Models
{
    public class FBUser
    {
        public long userId { get; set; }
        public string accessToken { get; set; }
        public string userName { get; set; }
        public string gender { get; set; }

        public FBUser(long userId, string accessToken, string userName, string gender)
        {
            this.userId = userId;
            this.accessToken = accessToken;
            this.userName = userName;
            this.gender = gender;
        }
    }
}