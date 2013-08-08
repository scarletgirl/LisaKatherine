using System.ComponentModel.DataAnnotations;
namespace LisaKatherine.Models
{
    [MetadataType(typeof(ArticlesMD))]
    public partial class Articles
    {
        public class ArticlesMD
        {
            [Display(Name = "Headline")]
            [StringLength(255, MinimumLength = 6)]
            [Required()]
            public object headline { get; set; }

            [Display(Name = "Strap Line")]
            [StringLength(800, MinimumLength = 0)]
            public object strapline { get; set; }


            [Required()]
            [Display(Name = "Article Body")]
            public object body { get; set; }

        }

        public Users user { get; set; }
        public ArticleTypes articleType { get; set; }
    }
}