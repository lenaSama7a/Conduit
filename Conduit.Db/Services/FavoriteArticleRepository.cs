using Conduit.Db.DBContexts;
using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.Services
{
    public class FavoriteArticleRepository : IFavoriteArticleRepository
    {
        private ConduitContext _context;
        private IUserRepository _userRepository;
        public FavoriteArticleRepository(ConduitContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public IQueryable<string> GetAllFavoriteArticlesForUserAsync(int userId)
        {
            var favoriteArticles = _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.FavoriteArticles,
                u => u.Id,
                fa => fa.UserId,
                (u, fa) => new { User = u, FavoriteArticle = fa })
            .Join(_context.Articles,
                fa => fa.FavoriteArticle.ArticleId,
                a => a.Id,
                (fa, a) => new { Article = a })
            .Select(a => "Id:" + a.Article.Id + " " + a.Article.Title + " - " + a.Article.Tag + " - " + a.Article.Body);
            
            return favoriteArticles;

        }

        public async Task<FavoriteArticle> AddFavoriteArticleAsync(int articleId, int userId)
        {
            var favoriteArticle = new FavoriteArticle();
            {
                favoriteArticle.ArticleId = articleId;
                favoriteArticle.UserId = userId;
            }
            _context.FavoriteArticles.Add(favoriteArticle);
            await _userRepository.SaveChangesAsync();
            return favoriteArticle;
        }

        public bool IsDuplicatedFavoriteArticle(int userId, int articleId)
        {
            var favoriteArticleRow = _context.FavoriteArticles.Find(userId, articleId);
            if (favoriteArticleRow != null)
                return true;
            return false;
        }

        public async Task DeleteFavoriteArticleAsync(int userId, int articleId)
        {
            var favoriteArticleToDelete = _context.FavoriteArticles.Where(d => d.UserId == userId && d.ArticleId == articleId).FirstOrDefault();
            if (favoriteArticleToDelete != null)
            {
                _context.FavoriteArticles.Remove(favoriteArticleToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> FavoriteArticleExistsAsync(int articleId)
        {
            return await _context.FavoriteArticles.AnyAsync(c => c.ArticleId == articleId);
        }
    }
}
