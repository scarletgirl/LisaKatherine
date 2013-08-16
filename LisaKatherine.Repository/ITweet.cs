namespace LisaKatherine.Interface
{
    using System;

    public interface ITweet
    {
        long TweetId { get; set; }

        string Text { get; set; }

        DateTime CreatedAt { get; set; }

        Uri Url { get; set; }
    }
}