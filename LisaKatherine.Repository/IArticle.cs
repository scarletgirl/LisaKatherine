namespace LisaKatherine.Interface
{
    using System;

    public interface IArticle
    {
        Int32 ArticleId { get; set; }

        String Headline { get; set; }

        String Strapline { get; set; }

        String Body { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DatePublished { get; set; }

        Boolean IsPublished { get; set; }

        Guid? Userid { get; set; }

        int? ArticleTypeId { get; set; }

        IUser User { get; set; }

        IArticleType ArticleType { get; set; }
    }
}