namespace LisaKatherine.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using LisaKatherine.Models;

    public class FacebookController : Controller
    {
        public PartialViewResult FacebookLogin(int id)
        {
            var user = (FacebookUser)this.Session["FBUser"];

            if (user == null)
            {
                this.ViewBag.FBLoginButton = FbAuthHandler.GenerateLoginUrl(id);
                this.ViewBag.FBLoginButtonVisible = true;
            }
            else
            {
                this.ViewBag.FBLoginButtonVisible = false;
                this.ViewBag.FBUserNameLabel = "Hello, " + user.name;
                this.ViewBag.FBUserImage = "https://graph.facebook.com/" + user.facebookId + "/picture";
            }

            return this.PartialView("_FacebookLogin");
        }

        public PartialViewResult FacebookComments(int id)
        {
            var user = (FacebookUser)this.Session["FBUser"];
            if (user != null)
            {
                IEnumerable<FacebookComment> comments = new FacebookService().GetCommentsForArticle(id);

                return this.PartialView("_FacebookComments", comments);
            }
            return null;
        }

        private bool FacebookIsLoggedIn()
        {
            return true;
        }
    }
}