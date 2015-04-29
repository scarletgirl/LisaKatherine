namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface ITwitterUrl
    {
        string DisplayUrl { get; set; }

        string ExpandedUrl { get; set; }

        IEnumerable<int> Indices { get; set; }

        string Url { get; set; }
    }
}