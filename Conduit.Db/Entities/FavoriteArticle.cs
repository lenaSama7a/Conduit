using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Conduit.Db.Entities
{
    public class FavoriteArticle
    {
        public Article? Article { get; set; }
        public int ArticleId { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
