using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LisaKatherine.Models
{
    public class TwitterService
    {
        public List<Tweet> GetTweets()
        {
            var xml = GetXDocument();
            var tweetList = from x in xml.Descendants("item")
                         select new
                         {
                             Title = x.Element("title").Value,
                             Description = x.Element("description").Value,
                             PubDate = x.Element("pubDate").Value,
                             Link = x.Element("link").Value

                         };

            return tweetList.Select(t => new Tweet {Description = t.Description, Link = t.Link, PubDate = Convert.ToDateTime(t.PubDate), Title = t.Title}).ToList();
        }

        public XDocument GetXDocument()
        {
            var rdr = new XmlTextReader("https://api.twitter.com/1/statuses/user_timeline.rss?screen_name=scarlet_girl");
            var xml = new XmlDocument();
            xml.Load(rdr);
            return ToXDocument(xml);
        }

        public XDocument ToXDocument(XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

    }
}
