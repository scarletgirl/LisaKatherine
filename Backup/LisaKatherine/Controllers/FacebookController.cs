using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using LisaKatherine.Models;
using System.Dynamic;

namespace LisaKatherine.Controllers
{
    public class FacebookController : Controller
    {

        private bool FacebookIsLoggedIn()
        {

            return true;
        }

        public PartialViewResult FacebookLogin(int id)
        {
            FacebookUser user = (FacebookUser)Session["FBUser"];

            if (user == null)
            {
                ViewBag.FBLoginButton = FBAuthHandler.generateLoginUrl(id);
                ViewBag.FBLoginButtonVisible = true;
            }
            else
            {
                ViewBag.FBLoginButtonVisible = false;
                ViewBag.FBUserNameLabel = "Hello, " + user.name;
                ViewBag.FBUserImage = "https://graph.facebook.com/" + user.facebookId + "/picture";
            }

            return PartialView("_FacebookLogin");
        }

        public PartialViewResult FacebookComments(int id)
        {
            FacebookUser user = (FacebookUser)Session["FBUser"];
            if (user != null)
            {
                var comments = new FacebookService().GetCommentsForArticle(id);

                return PartialView("_FacebookComments", comments);
            }
            return null;
        }
    }
}
