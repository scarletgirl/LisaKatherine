namespace LisaKatherine.Interface
{
    using System;
    using System.Collections.Generic;

    public class Tweet : ITweet
    {
        public Tweet()
        {
        }

        public Tweet(long tweetId, string text, DateTime createdAt, IEnumerable<ITwitterUrl> urls, IEnumerable<ITwitterMention> mentions, IEnumerable<ITwitterHashTag> hashtags)
        {
            this.TweetId = tweetId;
            this.Text = text;
            this.CreatedAt = createdAt;
            this.HashTags = hashtags;
            this.Mentions = mentions;
            this.Urls = urls;
        }

        public long TweetId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<ITwitterUrl> Urls { get; set; }

        public IEnumerable<ITwitterMention> Mentions { get; set; }

        public IEnumerable<ITwitterHashTag> HashTags { get; set; }
    }
}