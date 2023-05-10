using System.ComponentModel.DataAnnotations;

namespace AccountManager.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(50)]
        public string? Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50)]
        public string? Password { get; set; }
    }
}