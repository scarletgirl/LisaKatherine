namespace LisaKatherine.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using LisaKatherine.Models;
    using LisaKatherine.Models.Extensions;

    public class FbAuthHandler
    {
        private const string RedirectUrl = "http://localhost/Blog/FBAuthorise";

        public static string GenerateLoginUrl(int id)
        {
            var extendedPermissions = new[] { "publish_stream", "offline_access", "publish_actions" };

            var parameters = new Dictionary<string, object>();

            var scope = new StringBuilder();
            scope.Append(string.Join(",", extendedPermissions));

            parameters["scope"] = scope.ToString();
            parameters["response_type"] = "code";
            parameters["display"] = "popup";

            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(RedirectUrl + "/" + id + "/");
            oAuthClient.AppId = ConfigurationManager.AppSettings["Facebook_API_Key"];
            oAuthClient.AppSecret = ConfigurationManager.AppSettings["Facebook_API_Secret"];
            Uri loginUri = oAuthClient.GetLoginUrl(parameters);
            return loginUri.AbsoluteUri;
        }

        public static void HandleOAuthResult(HttpRequest request, int id)
        {
            FacebookOAuthResult oAuthResult;
            if (FacebookOAuthResult.TryParse(request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    string code = request.Params["code"];
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current)
                                          {
                                              RedirectUri =
                                                  new Uri(
                                                  RedirectUrl + "/"
                                                  + id + "/"),
                                              AppId =
                                                  ConfigurationManager
                                                  .AppSettings[
                                                      "Facebook_API_Key"
                                                  ],
                                              AppSecret =
                                                  ConfigurationManager
                                                  .AppSettings[
                                                      "Facebook_API_Secret"
                                                  ]
                                          };
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);

                    string accessToken = tokenResult.access_token;

                    var facebookClient = new FacebookClient(accessToken);
                    dynamic me = facebookClient.Get("me");
                    long facebookId = Convert.ToInt64(me.id);

                    var facebookService = new FacebookService();
                    FacebookUser fbUser = facebookService.AddFacebookUser(facebookId, accessToken, me.name, me.gender);
                    HttpContext.Current.Session["FBUser"] = fbUser;
                    var publishedArticleService = new PublishedArticleService();
                    PublishedArticles article = publishedArticleService.GetPublishedArticle(id);
                    HttpContext.Current.Response.Redirect(
                        "/Blog/Details/" + article.headline.Replace(" ", "_") + "/" + article.articleId + "/");
                }
            }
        }
    }
}