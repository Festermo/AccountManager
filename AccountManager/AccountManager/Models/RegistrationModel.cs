using System.ComponentModel.DataAnnotations;

namespace AccountManager.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field is too long")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field is too long")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"[a-zA-Z]{1,50}", ErrorMessage = "Wrong name format")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"[a-zA-Z]{1,50}", ErrorMessage = "Wrong surname format")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Phone]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string? Email { get; set; }
    }
}