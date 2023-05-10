using System.ComponentModel.DataAnnotations;

namespace AccountManager.Models
{
    public class AccountModel
    {
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