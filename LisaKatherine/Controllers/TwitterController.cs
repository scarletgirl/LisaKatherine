namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Services;

    public class TwitterController : Controller
    {
        private readonly TwitterService _twitterService = new TwitterService();

        //public PartialViewResult RenderTwitterList(int? page)
        //{
        //    //var tweets = _twitterService.GetTweets();
        //    //return PartialView("_TwitterFeed", tweets);
        //}
    }
}