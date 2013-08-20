namespace LisaKatherine.Interface
{
    using System.Collections.Generic;

    public class TwitterHashTag : ITwitterHashTag
    {
        public TwitterHashTag()
        {
        }

        public TwitterHashTag(string text, IEnumerable<int> indices)
        {
            this.Text = text;
            this.Indices = indices;
        }

        public string Text { get; set; }

        public IEnumerable<int> Indices { get; set; }
    }
}