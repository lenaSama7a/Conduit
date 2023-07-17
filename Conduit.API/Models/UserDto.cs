using System.ComponentModel.DataAnnotations;

namespace Conduit.API.Models
{
    /// <summary>
    /// A DTO for a user without Articles & comments
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? bio { get; set; }
    }
}
