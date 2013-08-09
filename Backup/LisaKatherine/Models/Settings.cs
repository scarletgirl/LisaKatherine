using System;
using System.Linq;
using System.Web.Configuration;
using System.Collections.Generic;

namespace LisaKatherine.Models
{
    public class Settings
    {
        public static int AdminPagerCount()
        {
            return Convert.ToInt32(WebConfigurationManager.AppSettings["AdminPagerCount"]);
        }

        public static string FlickrSetUrl(long id)
        {
            return String.Format(WebConfigurationManager.AppSettings["FlickrSetUrl"], id);
        }
    }
}