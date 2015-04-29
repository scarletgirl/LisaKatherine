namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public class TwitterMention : LisaKatherine.Interface.ITwitterMention
    {
        public TwitterMention()
        {
        }

        public TwitterMention(int id, string screenName, string Name, IEnumerable<int> indices)
        {
        }

        public int Id { get; set; }

        public string ScreenName { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> Indicies { get; set; }
    }
}