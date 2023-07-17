using Conduit.Db.Entities;

namespace Conduit.Db.Services
{
    public interface IFollowsRepository
    {
        IQueryable<string> GetFollowersForUser(int userId);
        IQueryable<string> GetFolloweesForUser(int userId);
        Task<Follow> FollowUserAsync(int followerId, int followeeId);
        bool IsDuplicatedFollowee(int followerId, int followeeId);
        Task<bool> DeleteFollowerAsync(int followerId, int currentUserId);
        Task<bool> DeleteFolloweeAsync(int followeeId, int currentUserId);
    }
}
