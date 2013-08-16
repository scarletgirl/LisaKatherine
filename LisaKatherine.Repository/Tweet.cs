namespace LisaKatherine.Interface
{
    using System;

    public class Tweet : ITweet
    {
        public Tweet()
        {
        }

        public Tweet(long tweetId, string text, DateTime createdAt, Uri url)
        {
            this.TweetId = tweetId;
            this.Text = text;
            this.CreatedAt = createdAt;
            this.Url = url;
        }

        public long TweetId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public Uri Url { get; set; }
    }
}