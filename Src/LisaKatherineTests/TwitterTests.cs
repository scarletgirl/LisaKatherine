namespace LisaKatherineTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LisaKatherine.Interface;
    using LisaKatherine.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class TwitterTests
    {
        [TestMethod]
        public void GetTwitterResponse()
        {
            var t = new TwitterService();
            string json = t.GetTimeLine();

            var p = JsonConvert.DeserializeObject<JArray>(json);

            JToken token = p[0];
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void GetTweetList()
        {
            string tweetString = Utils.GetFileContents("tweets.txt");

            var t = new TwitterService();
            List<ITweet> list = t.GetTweets(tweetString);
            Assert.IsNotNull(list);

            Assert.AreEqual(3, list.Count);

            ITweet tweet = list[0];
            Assert.AreEqual(366658742693007360, tweet.TweetId);
            Assert.AreEqual("Finished Walk with #walkmeter, on Long Furlong route, time 3:35:56, 9.73 miles, see http://t.co/50C9VAiqd8, average 22:12.", tweet.Text);
            Assert.AreEqual(DateTime.Parse("11/08/2013 21:33:51"), tweet.CreatedAt);

            tweet = list[1];
            Assert.AreEqual(366263614375608320, tweet.TweetId);
            Assert.AreEqual("Cocking!!!! http://t.co/9YBBXDHIue", tweet.Text);
            Assert.AreEqual(DateTime.Parse("10/08/2013 19:23:45"), tweet.CreatedAt);

            tweet = list[2];
            Assert.AreEqual(365761998572630016, tweet.TweetId);
            Assert.AreEqual("The Red Balloons http://t.co/qobDs201XJ", tweet.Text);
            Assert.AreEqual(DateTime.Parse("09/08/2013 10:10:30"), tweet.CreatedAt);
        }

        [TestMethod]
        public void GetTwitter()
        {
            string tweetString = Utils.GetFileContents("tweets.txt");
            var t = new TwitterService();
            var twitter = t.GetTwitter(tweetString);

            Assert.AreEqual("Heading to single middle age at a depressingly faster pace!", twitter.Description);
            Assert.AreEqual("scarlet_girl", twitter.ScreenName);
            Assert.AreEqual("http://twitter.com/scarlet_girl", twitter.Link);
            Assert.AreEqual("Lisa Katherine", twitter.Title);
            Assert.AreEqual(3, twitter.Tweets.Count());
            Assert.AreEqual(19838040, twitter.UserId);
        }
    }
}