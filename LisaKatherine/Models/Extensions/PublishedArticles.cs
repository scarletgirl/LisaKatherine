
namespace LisaKatherine.Models
{

    public partial class PublishedArticles
    {
        public Users user { get; set; }
        public ArticleTypes articleType { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}