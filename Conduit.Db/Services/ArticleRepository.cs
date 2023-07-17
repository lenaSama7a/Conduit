using Azure;
using Conduit.Db.DBContexts;
using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private ConduitContext _context;
        private IUserRepository _userRepository;
        public ArticleRepository(ConduitContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Article>> GetArticlesForUserAsync(int userId)
        {
            return await _context.Articles.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int ArticleId)
        {
            return await _context.Articles
                           .Where(p => p.Id == ArticleId)
                           .FirstOrDefaultAsync();
        }

        public async Task<Article?> GetArticleForUserAsync(int userId, int ArticleId)
        {
            return await _context.Articles
                           .Where(p => p.UserId == userId && p.Id == ArticleId)
                           .FirstOrDefaultAsync();
        }

        public async Task AddArticleForUserAsync(int userId, Article article)
        {
            var user = await _userRepository.GetUserByIdAsync(userId,false);
            if (user != null)
            {
                user.Articles.Add(article);
            }
        }

        public async Task<IEnumerable<Article>> GetArticlesByTagAsync(string tagOrTitle)
        {
            var collection = _context.Articles as IQueryable<Article>;

            if (!string.IsNullOrWhiteSpace(tagOrTitle))
            {
                tagOrTitle = tagOrTitle.Trim();
                collection = collection.Where(a => a.Tag.Contains(tagOrTitle) || a.Title.Contains(tagOrTitle));
            }
            return await collection.ToListAsync();
        }

        public async Task<Article> UpdateArticleAsync(Article article)
        {
            var original = await _context.Articles.FirstOrDefaultAsync(d => d.Id == article.Id);
            original.Title = article.Title;
            original.Tag = article.Tag;
            original.Body = article.Body;

            await _context.SaveChangesAsync();
            return article;
        }
        public int getUserIdForArticle(int articleId)
        {
            var article = _context.Articles.Where(u => u.Id == articleId).FirstOrDefault();
            if (article != null)
            {
                return article.UserId;
            }
            return 0;
        }
        public async Task<bool> ArticleExistsAsync(int articleId)
        {
            return await _context.Articles.AnyAsync(c => c.Id == articleId);
        }

        public void DeleteArticle(Article article)
        {
                _context.Articles.Remove(article);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
