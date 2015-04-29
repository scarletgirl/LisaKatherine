namespace LisaKatherine.Interface
{
    using System;

    public interface IArticle
    {
        int ArticleId { get; set; }

        string Headline { get; set; }

        string Strapline { get; set; }

        string Body { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DatePublished { get; set; }

        bool IsPublished { get; set; }

        Guid? Userid { get; set; }

        int? ArticleTypeId { get; set; }

        IUser User { get; set; }

        IArticleType ArticleType { get; set; }
    }
}