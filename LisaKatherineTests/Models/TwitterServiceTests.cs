
using System;
using LisaKatherine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LisaKatherineTests.Models
{
    [TestClass]
    public class TwitterServiceTests
    {
        private readonly DateTime _pubDate = Convert.ToDateTime("1/1/2001");
        private const string Link = "http://twitter";
        private const string Title = "the title";
        private const string Description = "the description";
        private readonly TwitterService _twitterservice = new TwitterService();

        [TestMethod]
        public void CreateModelTest()
        {
            var twitter = new Twitter();
            Assert.IsNotNull(twitter);
        }

        [TestMethod]
        public void TwitterObjectHasAttributes()
        {
            var twitter = new Twitter();
            var titleProperty = twitter.GetType().GetProperty("Title");
            var linkProperty = twitter.GetType().GetProperty("Link");
            var descriptionProperty = twitter.GetType().GetProperty("Description");
            var tweets = twitter.GetType().GetProperty("Tweets");
             
            Assert.IsNotNull(titleProperty, "Title Property Missing");
            Assert.IsNotNull(linkProperty, "Link property missing");
            Assert.IsNotNull(descriptionProperty, "Description property missing");
            Assert.IsNotNull(tweets, "tweets property missing");
        }

        [TestMethod]
        public void TweetObjectTest()
        {
            var tweet = new Tweet {Title = Title, Description = Description, PubDate = _pubDate, Link = Link};

            Assert.IsTrue(tweet.Description == Description);
            Assert.IsTrue(tweet.Title == Title);
            Assert.IsTrue(tweet.Link == Link);
            Assert.IsTrue(tweet.PubDate == _pubDate);
        }

        [TestMethod]
        public void CheckReturnedXml()
        {
            var xml = _twitterservice.GetXDocument();
            Assert.IsNotNull(xml, "xml is null");
        }

        [TestMethod]
        public void ReturnsTweetList()
        {
            var tweets = _twitterservice.GetTweets();
            Assert.IsTrue(tweets.Count > 0);
        }
    }
}
