namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface ITwitter
    {
        string Title { get; set; }

        string Link { get; set; }

        string Description { get; set; }

        List<Tweet> Tweets { get; set; }

        long UserId { get; set; }
    }
}