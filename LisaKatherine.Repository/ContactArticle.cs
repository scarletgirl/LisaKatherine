namespace LisaKatherine.Interface
{
    public class ContactArticle : IContactArticle
    {
        public IContact Contact { get; set; }

        public IPublishedArticle PublishedArticle { get; set; }
    }
}