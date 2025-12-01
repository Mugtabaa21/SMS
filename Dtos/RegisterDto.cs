using mewo.Data; // specific namespace where AllowedRole enum is defined
using System.ComponentModel.DataAnnotations;

namespace mewo.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public AllowedRole Role { get; set; }
    }
}