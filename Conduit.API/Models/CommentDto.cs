using Conduit.Db.Entities;

namespace Conduit.API.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}
