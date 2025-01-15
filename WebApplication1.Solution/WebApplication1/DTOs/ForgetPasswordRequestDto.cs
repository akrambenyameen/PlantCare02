using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class ForgetPasswordRequestDto
    {
        [EmailAddress]
        [Required]
        public  string Email { get; set; }
    }
}
