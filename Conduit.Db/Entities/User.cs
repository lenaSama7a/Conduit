using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.Entities
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(400)]
        public string? Bio { get; set; }
        public ICollection<Article>? Articles { get; set; } = new List<Article>();
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<FavoriteArticle>? FavoriteArticles { get; set; } = new List<FavoriteArticle>();
        public ICollection<Follow>? Followers { get; set; } = new List<Follow>();
        public ICollection<Follow>? Followees { get; set; } = new List<Follow>();

    }
}
