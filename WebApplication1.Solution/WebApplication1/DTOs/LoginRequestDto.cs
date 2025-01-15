using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class LoginRequestDto
    {
        [EmailAddress(ErrorMessage = "Email address is required ")]

        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one letter and one number.")]
        public string password { get; set; }
    }
}
