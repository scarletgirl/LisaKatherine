namespace LisaKatherine.Interface
{
    using System;
    using System.Collections.Generic;

    public interface ITweet
    {
        long TweetId { get; set; }

        string Text { get; set; }

        DateTime CreatedAt { get; set; }

        IEnumerable<ITwitterUrl> Urls { get; set; }

        IEnumerable<ITwitterMention> Mentions { get; set; }

        IEnumerable<ITwitterHashTag> HashTags { get; set; }
    }
}