using System;
using System.Collections.Generic;

namespace LisaKatherine.Models
{
    public class Twitter
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public List<Tweet> Tweets { get; set; } 
    }

    public class Tweet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Link { get; set; }
    }
}