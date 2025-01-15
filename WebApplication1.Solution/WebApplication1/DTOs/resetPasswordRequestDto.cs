using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class resetPasswordRequestDto
    {
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one letter, one number, and be at least 8 characters long.")]
        public string newPassword { get; set; }

        [Required]
        [Compare("newPassword", ErrorMessage = "Passwords do not match.")]
        public string confirmPassword { get; set; }

       // public string token { get; set; }

        //[EmailAddress]
        //[Required]
        //public string email { get; set; }
    }
}
