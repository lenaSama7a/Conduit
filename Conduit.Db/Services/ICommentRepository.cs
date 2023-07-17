using Conduit.Db.Entities;

namespace Conduit.Db.Services
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsForArticleAsync(int articleId);
        Task<Comment?> GetCommentAsync(int commentId);
        Task AddCommentForArticleAsync(int userId, int articleId, Comment comment);
        int getUserIdForComment(int commentId);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<Comment?> GetCommentForUserAsync(int userId, int commentId);
        void DeleteComment(Comment comment);
        Task<bool> CommentExistsAsync(int commentId);
    }
}
