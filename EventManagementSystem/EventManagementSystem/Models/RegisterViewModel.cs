using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Middlename { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
