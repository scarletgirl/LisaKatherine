namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface ITwitterHashTag
    {
        string Text { get; set; }

        IEnumerable<int> Indices { get; set; }
    }
}