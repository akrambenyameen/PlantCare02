using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class UserDto
    {
        public string token  { get; set; }
        [Required (ErrorMessage = "name is required")]
        public string  name { get; set; }

        [EmailAddress(ErrorMessage = "Email address is required ")]
        public string email { get; set; }
    }
}
