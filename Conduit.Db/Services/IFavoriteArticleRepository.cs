using Conduit.Db.Entities;

namespace Conduit.Db.Services
{
    public interface IFavoriteArticleRepository
    {
        IQueryable<string> GetAllFavoriteArticlesForUserAsync(int userId);
        Task<FavoriteArticle> AddFavoriteArticleAsync(int articleId, int userId);
        bool IsDuplicatedFavoriteArticle(int userId, int articleId);
        Task DeleteFavoriteArticleAsync(int userId, int articleId);
        Task<bool> FavoriteArticleExistsAsync(int articleId);
    }
}
