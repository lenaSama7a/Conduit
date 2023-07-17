using Conduit.Db.DBContexts;
using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Conduit.Db.Services
{
    public class UserRepository : IUserRepository
    {
        private ConduitContext _context;
        public UserRepository(ConduitContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(c => c.UserName).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string? name, bool includeArticle)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAllUsersAsync();
            }
            var collection = _context.Users as IQueryable<User>;

            name = name.Trim();
            collection = collection.Where(a => a.UserName.Contains(name)
            || (a.Email != null && a.Email.Contains(name)));
            
            if(includeArticle)
            {
                return await collection.Include(c=> c.Articles).OrderBy(c => c.UserName).ToListAsync();

            }
            return await collection.OrderBy(c => c.UserName).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId, bool includeArticles)
        {
            if (includeArticles)
            {
                return await _context.Users.Include(c => c.Articles)
                    .Where(c => c.Id == userId).FirstOrDefaultAsync();
            }

            return await _context.Users
                  .Where(c => c.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ValidateuserName(string userName)
        {
            if(await _context.Users.AnyAsync(c => c.UserName == userName))
            {
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateEmail(string email, int userId)
        {
            if (await _context.Users.Where(c => c.Id != userId).AnyAsync(c => c.Email == email))
            {
                return false;
            }
            return true;
        }
        public bool ValidatePassword(string passWord)
        {
            if (passWord.Length < 8)
                return false;
            int validConditions = 0;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            if (validConditions == 2)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };    
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }

        public List<int> FolloweesIDs(int userId)
        {
            var followeesIDs = _context.Users.Join(_context.Follows, u => u.Id, f => f.FolloweeId,
                (u, f) => new { User = u, Follow = f })
                .Where(x => x.Follow.FollowerId == userId)
                .Select(a => a.User.Id).ToList();
            return followeesIDs;
        }

        public IEnumerable<string> GetArticlesofFollowees(int userId)
        {
            var followeesIDs = FolloweesIDs(userId);
            var Articles = _context.Articles.Where(a => followeesIDs.Contains(a.UserId)).ToList()
                .Join(_context.Users, f => f.UserId, u => u.Id, (f,u) => new { Article = f, User = u })
                .Select(x => "You Have a new article from: " + x.User.UserName + " -Title: " + x.Article.Title 
                + "  -Body " + x.Article.Body + " -Tag: " + x.Article.Tag).ToList();
                
            return Articles;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var original = await _context.Users.FirstOrDefaultAsync(d => d.Id == user.Id);
            _context.Entry(original).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public int GetIdByUserName(string userName)
        {
            var user = _context.Users.Where(d => d.UserName == userName).FirstOrDefault();
            return user.Id;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = _context.Users.Where(d => d.Id == userId).FirstOrDefault();
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExistsAsync(int userId) { 
            return await _context.Users.AnyAsync(c => c.Id == userId);
        }

        public async Task<bool> UserNameMatchesUserId(string? userName, int userId)
        {
            return await _context.Users.AnyAsync(c => c.Id == userId && c.UserName == userName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }


    }
}
