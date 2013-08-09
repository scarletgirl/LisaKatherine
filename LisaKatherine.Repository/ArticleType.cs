namespace LisaKatherine.Interface
{
    public class ArticleType : IArticleType
    {
        public ArticleType(int articleTypeId, string articleTypeName, int? sectionId)
        {
            this.SectionId = sectionId;
            this.ArticleTypeName = articleTypeName;
            this.ArticleTypeId = articleTypeId;
        }

        public int ArticleTypeId { get; set; }

        public string ArticleTypeName { get; set; }

        public int? SectionId { get; set; }
    }
}