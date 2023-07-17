using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Db.Entities
{
    public class Comment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(600)]
        public string Body { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public Article? Article { get; set; }
        public int ArticleId { get; set; }
    }
}
