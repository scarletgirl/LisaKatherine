namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public interface ITwitterMention
    {
        int Id { get; set; }

        IEnumerable<int> Indicies { get; set; }

        string Name { get; set; }

        string ScreenName { get; set; }
    }
}