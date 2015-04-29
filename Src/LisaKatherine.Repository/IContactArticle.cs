namespace LisaKatherine.Interface
{
    public interface IContactArticle
    {
        IContact Contact { get; set; }

        IPublishedArticle PublishedArticle { get; set; }
    }
}