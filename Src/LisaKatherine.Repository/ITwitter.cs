namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface ITwitter
    {
        string Title { get; set; }

        string Link { get; set; }

        string Description { get; set; }

        IEnumerable<ITweet> Tweets { get; set; }

        long UserId { get; set; }

        string ScreenName { get; set; }
    }
}