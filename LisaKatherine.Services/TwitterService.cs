namespace LisaKatherine.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using LisaKatherine.Interface;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

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

        public List<ITweet> GetTweets(string tweetString)
        {
            var p = JsonConvert.DeserializeObject<JArray>(tweetString);
            var list = new List<ITweet>();
            foreach (JToken token in p)
            {
                var t = new Tweet
                            {
                                Text = token.Value<string>("text"),
                                TweetId = token.Value<long>("id"),
                                Urls = this.GetUrlsFromTwitterToken(token),
                                HashTags = this.GetHashTagsFromTwitterToken(token),
                                Mentions = this.GetMentionsFromTwitterToken(token)
                            };
                var date = token.Value<string>("created_at");
                t.CreatedAt = DateTime.ParseExact(date, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);

                list.Add(this.ProcessTweet(t));
            }

            return list;
        }

        public ITwitter GetTwitter()
        {
            return this.GetTwitter(this.GetTimeLine());
        }

        public ITwitter GetTwitter(string tweetString)
        {
            var p = JsonConvert.DeserializeObject<JArray>(tweetString);
            var twitter = new Twitter();
            if (p.Any())
            {
                var user = p[0].Value<JToken>("user");
                twitter.UserId = user.Value<long>("id");
                twitter.Tweets = this.GetTweets(tweetString);
                twitter.Description = user.Value<string>("description");
                twitter.ScreenName = user.Value<string>("screen_name");
                twitter.Title = user.Value<string>("name");
                twitter.Link = string.Format("http://twitter.com/{0}", twitter.ScreenName);
            }

            return twitter;
        }

        public IEnumerable<ITwitterUrl> GetUrlsFromTwitterToken(JToken token)
        {
            var t = this.GetEntitiesForTweet(token).Value<JToken>("urls");

            if (t != null)
            {
                return
                    t.Select(
                        url =>
                        new TwitterUrl
                            {
                                DisplayUrl = url.Value<string>("display_url"),
                                ExpandedUrl = url.Value<string>("expanded_url"),
                                Url = url.Value<string>("url"),
                                Indices = this.GetIndicesOfEntity(url.Value<JToken>("indices"))
                            }).Cast<ITwitterUrl>().ToList();
            }
            return new List<ITwitterUrl>();
        }

        public IEnumerable<ITwitterHashTag> GetHashTagsFromTwitterToken(JToken token)
        {
            var t = this.GetEntitiesForTweet(token).Value<JToken>("hashtags");
            if (t != null)
            {
                return
                    t.Select(hashtag => new TwitterHashTag { Indices = this.GetIndicesOfEntity(hashtag.Value<JToken>("indices")), Text = hashtag.Value<string>("text") })
                     .Cast<ITwitterHashTag>()
                     .ToList();
            }
            return new List<ITwitterHashTag>();
        }

        public IEnumerable<ITwitterMention> GetMentionsFromTwitterToken(JToken token)
        {
            var t = this.GetEntitiesForTweet(token).Value<JToken>("mention");
            if (t != null)
            {
                return
                    t.Select(
                        mention =>
                        new TwitterMention
                            {
                                Id = mention.Value<int>("id"),
                                Indicies = this.GetIndicesOfEntity(mention.Value<JToken>("indices")),
                                Name = mention.Value<string>("name"),
                                ScreenName = mention.Value<string>("screen_name")
                            }).Cast<ITwitterMention>().ToList();
            }
            return new List<ITwitterMention>();
        }

        public JToken GetEntitiesForTweet(JToken token)
        {
            var entites = token.Value<JToken>("entities");
            return entites;
        }

        public IEnumerable<int> GetIndicesOfEntity(JToken token)
        {
            return token.Select(index => index.Value<int>()).ToList();
        }

        public ITweet ProcessTweet(ITweet tweet)
        {
            string original = tweet.Text;
            foreach (ITwitterUrl url in tweet.Urls)
            {
                List<int> indices = url.Indices.ToList();
                tweet.Text = tweet.Text.Replace(
                    original.Substring(indices[0], indices[1] - indices[0]), string.Format("<a href='{0}' target='_blank'>{1}</a>", url.Url, url.DisplayUrl));
            }

            foreach (ITwitterMention mention in tweet.Mentions)
            {
                List<int> indices = mention.Indicies.ToList();
                tweet.Text = tweet.Text.Replace(
                    original.Substring(indices[0], indices[1] - indices[0]),
                    string.Format("<a href='http://twitter.com/{0}' target='_blank'>{1}</a>", mention.ScreenName, original.Substring(indices[0], indices[1])));
            }

            foreach (ITwitterHashTag hashTag in tweet.HashTags)
            {
                List<int> indices = hashTag.Indices.ToList();
                tweet.Text = tweet.Text.Replace(
                    original.Substring(indices[0], indices[1] - indices[0]),
                    string.Format("<a href=https://twitter.com/search?q=%23{0}&src=hash' target='_blank'>{1}</a>", hashTag.Text, original.Substring(indices[0], indices[1])));
            }
            return tweet;
        }
    }
}