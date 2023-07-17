using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Db.Entities
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tag { get; set; }

        [Required]
        [MaxLength(600)]
        public string Body { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<FavoriteArticle>? FavoriteArticles { get; set; } = new List<FavoriteArticle>();
    }
}
