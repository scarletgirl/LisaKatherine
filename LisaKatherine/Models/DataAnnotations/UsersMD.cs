
using System.ComponentModel.DataAnnotations;


namespace LisaKatherine.Models
{
    [MetadataType(typeof(UsersMD))]
    public partial class Users
    {
        public class UsersMD {
            [Display(Name="Username")]
            [StringLength(50, MinimumLength = 6)]
            [Required()]
            public object username { get; set; }

            [Display(Name = "Password")]
            [StringLength(50, MinimumLength = 6)]
            [Required()]
            public object password { get; set; }

            [Display(Name = "First Name")]
            [StringLength(50, MinimumLength = 1)]
            [Required()]
            public object firstname { get; set; }

            [Display(Name = "Last Name")]
            [StringLength(50, MinimumLength = 1)]
            [Required()]
            public object lastname { get; set; }
        }
    }
}