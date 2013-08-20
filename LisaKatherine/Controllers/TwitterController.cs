namespace LisaKatherine.Controllers
{
    using System.Web.Mvc;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    public class TwitterController : Controller
    {
        private readonly TwitterService twitterService = new TwitterService();

        public PartialViewResult RenderTwitterList(int? page)
        {
            var twitter = (Twitter)this.twitterService.GetTwitter();
            return this.PartialView("TwitterFeed", twitter);
        }
    }
}