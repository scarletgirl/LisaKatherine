namespace LisaKatherine.Interface
{
    using System;

    public class PublishedArticle : Article, IPublishedArticle
    {
        public PublishedArticle(
            string headline,
            string strapline,
            string body,
            DateTime dateCreated,
            DateTime? datePublished,
            bool isPublished,
            int? articleTypeId,
            IUser user,
            IArticleType articleType)
            : base(headline, strapline, body, dateCreated, datePublished, isPublished, articleTypeId, user, articleType)
        {
        }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}