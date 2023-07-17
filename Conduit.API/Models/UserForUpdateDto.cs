using System.ComponentModel.DataAnnotations;

namespace Conduit.API.Models
{
    public class UserForUpdateDto
    {

        [Required(ErrorMessage = " you should provide an Email value")]
        public string Email { get; set; }

        [Required(ErrorMessage = " you should provide a password value")]
        public string Password { get; set; }
        public string? bio { get; set; }
    }
}
