namespace LisaKatherine.Services
{
    using OAuthTwitterWrapper;

    public class TwitterService
    {
        private readonly IOAuthTwitterWrapper oAuthTwitterWrapper;

        public TwitterService(IOAuthTwitterWrapper oAuthTwitterWrapper)
        {
            this.oAuthTwitterWrapper = oAuthTwitterWrapper;
        }

        public TwitterService()
        {
            this.oAuthTwitterWrapper = new OAuthTwitterWrapper();
        }

        public string GetTimeLine()
        {
            return this.oAuthTwitterWrapper.GetMyTimeline();
        }
    }
}