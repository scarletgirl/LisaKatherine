namespace LisaKatherine.Services
{
    using System;
    using System.Web.Configuration;

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