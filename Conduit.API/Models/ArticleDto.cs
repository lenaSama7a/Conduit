using System.ComponentModel.DataAnnotations;

namespace Conduit.API.Models
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }

    }
}
