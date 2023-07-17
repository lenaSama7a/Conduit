using Conduit.Db.DBContexts;
using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.Services
{
    public class FollowsRepository : IFollowsRepository
    {
        private ConduitContext _context;
        private IUserRepository _userRepository;
        public FollowsRepository(ConduitContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public IQueryable<string> GetFollowersForUser(int userId)
        {
            var followers = _context.Users.Join(_context.Follows, u => u.Id, f => f.FollowerId,
            (u, f) => new { User = u, Follow = f })
            .Where(x => x.Follow.FolloweeId == userId)
            .Select(a => "user Id:" + a.User.Id + " - user name: " + a.User.UserName);

            return followers;
        }

        public IQueryable<string> GetFolloweesForUser(int userId)
        {
            var followees = _context.Users.Join(_context.Follows, u => u.Id, f => f.FolloweeId,
            (u, f) => new { User = u, Follow = f })
            .Where(x => x.Follow.FollowerId == userId)
            .Select(a => "user Id:" + a.User.Id + " - user name: " + a.User.UserName);
            return followees;
        }

        public async Task<Follow> FollowUserAsync(int followerId, int followeeId)
        {
            var follow = new Follow();
            {
                follow.FollowerId = followerId;
                follow.FolloweeId = followeeId;
            }
            _context.Follows.Add(follow);
            await _userRepository.SaveChangesAsync();
            return follow;
        }

        public bool IsDuplicatedFollowee(int followerId, int followeeId)
        {
            var followRow = _context.Follows.Find(followerId, followeeId);
            if (followRow != null)
                return true;
            return false;
        }

        public async Task<bool> DeleteFollowerAsync(int followerId, int currentUserId)
        {
            var followerToDelete = _context.Follows.Where(d => d.FollowerId == followerId && d.FolloweeId == currentUserId).FirstOrDefault();
            if (followerToDelete != null)
            {
                _context.Follows.Remove(followerToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteFolloweeAsync(int followeeId, int currentUserId)
        {
            var followeeToDelete = _context.Follows.Where(d => d.FolloweeId == followeeId && d.FollowerId == currentUserId).FirstOrDefault();
            if (followeeToDelete != null)
            {
                _context.Follows.Remove(followeeToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
