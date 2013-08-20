namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public class TwitterUrl : ITwitterUrl
    {
        public TwitterUrl()
        {
        }

        public TwitterUrl(string url, string displayUrl, string expandedUrl, IEnumerable<int> indices)
        {
            this.Url = url;
            this.DisplayUrl = displayUrl;
            this.ExpandedUrl = expandedUrl;
            this.Indices = indices;
        }

        public string Url { get; set; }

        public string DisplayUrl { get; set; }

        public string ExpandedUrl { get; set; }

        public IEnumerable<int> Indices { get; set; }
    }
}