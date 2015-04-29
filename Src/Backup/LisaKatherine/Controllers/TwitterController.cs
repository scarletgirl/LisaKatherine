using System.Web.Mvc;
using LisaKatherine.Models;

namespace LisaKatherine.Controllers
{
    public class TwitterController : Controller
    {
        private readonly TwitterService _twitterService = new TwitterService();
        public PartialViewResult RenderTwitterList(int? page)
        {
            var tweets = _twitterService.GetTweets();
            return PartialView("_TwitterFeed", tweets);
        }

    }
}
