using Conduit.Db.Entities;

namespace Conduit.Db.Services
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetArticlesForUserAsync(int userId);
        Task<Article?> GetArticleByIdAsync(int ArticleId);
        Task<Article?> GetArticleForUserAsync(int userId, int ArticleId);
        Task AddArticleForUserAsync(int userId, Article article);
        Task<bool> ArticleExistsAsync(int articleId);
        Task<IEnumerable<Article>> GetArticlesByTagAsync(string tag);
        Task<Article> UpdateArticleAsync(Article article);
        int getUserIdForArticle(int articleId);
        void DeleteArticle(Article article);
        Task<bool> SaveChangesAsync();
    }
}
