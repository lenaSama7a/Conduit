using Conduit.Db.Entities;

namespace Conduit.API.Models
{
    public class UserWithArticlesDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? bio { get; set; }
        public ICollection<ArticleDto> Articles { get; set; } = new List<ArticleDto>();
    }
}
