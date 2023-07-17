using Conduit.Db.Entities;
using System.ComponentModel.DataAnnotations;

namespace Conduit.API.Models
{
    public class ArticleForCreationDto
    {
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Body { get; set; }
    }
}
