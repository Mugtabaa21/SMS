
using System.ComponentModel.DataAnnotations;

namespace mewo.Dtos
{
    public class SignInDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
        
    }
}
