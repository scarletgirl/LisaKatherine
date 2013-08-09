namespace LisaKatherine.Interface
{
    public interface IArticleType
    {
        int ArticleTypeId { get; set; }

        string ArticleTypeName { get; set; }

        int? SectionId { get; set; }
    }
}