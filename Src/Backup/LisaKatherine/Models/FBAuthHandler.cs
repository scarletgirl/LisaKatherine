using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using System.Text;
using System.Configuration;
using LisaKatherine.Models;


namespace LisaKatherine.Models
{
    public class FBAuthHandler
    {
        private const string redirectUrl = "http://localhost/Blog/FBAuthorise";

        public static string generateLoginUrl(int id)
        {
            string[] extendedPermissions = new[] { "publish_stream", "offline_access", "publish_actions" };

            var parameters = new Dictionary<string, object>();

            StringBuilder scope = new StringBuilder();
            scope.Append(string.Join(",", extendedPermissions));

            parameters["scope"] = scope.ToString();
            parameters["response_type"] = "code";
            parameters["display"] = "popup";

            FacebookOAuthClient oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(redirectUrl + "/" + id + "/");
            oAuthClient.AppId = ConfigurationManager.AppSettings["Facebook_API_Key"];
            oAuthClient.AppSecret = ConfigurationManager.AppSettings["Facebook_API_Secret"];
            Uri loginUri = oAuthClient.GetLoginUrl(parameters);
            return loginUri.AbsoluteUri;
        }

        public static void handleOAuthResult(HttpRequest request, int id)
        {
            FacebookOAuthResult oAuthResult;
            if (FacebookOAuthResult.TryParse(request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    string code = request.Params["code"];
                    FacebookOAuthClient oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
                    oAuthClient.RedirectUri = new Uri(redirectUrl + "/" + id + "/");
                    oAuthClient.AppId = ConfigurationManager.AppSettings["Facebook_API_Key"];
                    oAuthClient.AppSecret = ConfigurationManager.AppSettings["Facebook_API_Secret"];
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);

                    string accessToken = tokenResult.access_token;

                    FacebookClient facebookClient = new FacebookClient(accessToken);
                    dynamic me = facebookClient.Get("me");
                    long facebookId = Convert.ToInt64(me.id);

                    var facebookService = new FacebookService();
                    FacebookUser fbUser = facebookService.AddFacebookUser(facebookId, accessToken, me.name, me.gender);
                    HttpContext.Current.Session["FBUser"] = fbUser;
                    var _publishedArticleService = new PublishedArticleService();
                    var article = _publishedArticleService.GetPublishedArticle(id);
                    System.Web.HttpContext.Current.Response.Redirect("/Blog/Details/" + article.headline.Replace(" ", "_") + "/" + article.articleId + "/");
                }
            }
        }
    }
}