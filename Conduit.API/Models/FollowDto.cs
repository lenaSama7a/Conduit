using Conduit.Db.Entities;

namespace Conduit.API.Models
{
    public class FollowDto
    {
        public int FollowerId { get; set; }
        public int FolloweeId { get; set; }
    }
}
