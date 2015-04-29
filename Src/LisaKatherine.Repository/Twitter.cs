namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public class Twitter : ITwitter
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public IEnumerable<ITweet> Tweets { get; set; }

        public long UserId { get; set; }

        public string ScreenName { get; set; }
    }
}