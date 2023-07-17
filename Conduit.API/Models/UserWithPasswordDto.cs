using System.ComponentModel.DataAnnotations;

namespace Conduit.API.Models
{
    public class UserWithPasswordDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " you should provide a name value")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " you should provide an Email value")]
        public string Email { get; set; }

        [Required(ErrorMessage = " you should provide a password value")]
        public string Password { get; set; }
        public string? bio { get; set; }
    }
}
