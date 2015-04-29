using System;
using System.Collections.Generic;
using System.Linq;

namespace LisaKatherine.Models
{
    public class FlickrImage
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Large_Url { get; set; }
        public string Medium_Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string ThumbnailUrl { get; set; }
        public int ThumbHeight { get; set; }
        public int ThumbWidth { get; set; }
        public Orientation Orientation { get; set; }
    }
}