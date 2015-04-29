namespace LisaKatherine.Interface
{
    using System;

    public class Article : IArticle
    {
        public Article(
            string headline,
            string strapline,
            string body,
            DateTime dateCreated,
            DateTime? datePublished,
            bool isPublished,
            int? articleTypeId,
            IUser user,
            IArticleType articleType)
        {
            this.Headline = headline;
            this.Strapline = strapline;
            this.Body = body;
            this.DateCreated = dateCreated;
            this.DatePublished = datePublished;
            this.IsPublished = isPublished;
            this.ArticleTypeId = articleTypeId;
            this.User = user;
            this.ArticleType = articleType;
        }

        public int ArticleId { get; set; }

        public string Headline { get; set; }

        public string Strapline { get; set; }

        public string Body { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DatePublished { get; set; }

        public bool IsPublished { get; set; }

        public Guid? Userid { get; set; }

        public int? ArticleTypeId { get; set; }

        public IUser User { get; set; }

        public IArticleType ArticleType { get; set; }
    }
}