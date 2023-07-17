using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Db.Entities
{
    public class Follow
    {
        public User Follower { get; set; }
        public int FollowerId { get; set; }
        public User Followee { get; set; }
        public int FolloweeId { get; set; }
    }
}
