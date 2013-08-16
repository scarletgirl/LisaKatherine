namespace LisaKatherine.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

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
            string s = this.GetTimeLine();
            var p = JsonConvert.DeserializeObject<JArray>(s);
            var list = new List<ITweet>();
            foreach (JToken token in p)
            {
                var t = new Tweet();
                t.Text = token.Value<string>("text");
                t.TweetId = token.Value<long>("id");
                var date = token.Value<string>("created_at");
                t.CreatedAt = DateTime.ParseExact(date, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);
                list.Add(t);
            }

            return list;
        }
    }
}