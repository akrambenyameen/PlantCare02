using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters long.")]
        public string name { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "Password must include [A-Z], [a-z], [0-9], and be ≥ 8 characters long.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
