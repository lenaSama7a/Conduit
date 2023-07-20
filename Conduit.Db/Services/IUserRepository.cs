using Conduit.Db.Entities;
namespace Conduit.Db.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByNameAsync(string? name, bool includeArticle);
        Task<User?> GetUserByIdAsync(int userId, bool includeArticles);
        Task<User> CreateUserAsync(User user);
        Task<bool> ValidateuserName(string userName);
        Task<User> UpdateUserAsync(User user);
        int GetIdByUserName(string userName);
        Task DeleteUserAsync(int userId);
        Task<bool> UserExistsAsync(int userId);
        IEnumerable<string> GetArticlesofFollowees(int userId);
        Task<bool> UserNameMatchesUserId(string? userName, int userId);
        Task<bool> SaveChangesAsync();
    }
}
