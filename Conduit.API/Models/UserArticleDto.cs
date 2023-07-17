using Conduit.Db.Entities;

namespace Conduit.API.Models
{
    public class UserArticleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
