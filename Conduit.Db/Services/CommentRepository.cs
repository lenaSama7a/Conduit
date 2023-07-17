using Conduit.Db.DBContexts;
using Conduit.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Db.Services
{
    public class CommentRepository : ICommentRepository
    {
        private ConduitContext _context;
        private IUserRepository _userRepository;
        private IArticleRepository _articleRepository;
        public CommentRepository(ConduitContext context, IUserRepository userRepository, IArticleRepository articleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsForArticleAsync(int articleId)
        {
            return await _context.Comments.Where(u => u.ArticleId == articleId).ToListAsync();
        }

        public async Task<Comment?> GetCommentAsync(int commentId)
        {
            return await _context.Comments
                           .Where(p => p.Id == commentId)
                           .FirstOrDefaultAsync();
        }

        public async Task AddCommentForArticleAsync(int userId, int articleId, Comment comment)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, false);
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            comment.ArticleId = articleId;
            comment.UserId = userId;
            if (user != null && article != null)
            {
                user.Comments.Add(comment);
            }
        }

        public int getUserIdForComment(int commentId)
        {
            var comment = _context.Comments.Where(u => u.Id == commentId).FirstOrDefault();
            if (comment != null)
            {
                return comment.UserId;
            }
            return 0;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            var original = await _context.Comments.FirstOrDefaultAsync(d => d.Id == comment.Id);
            original.Body = comment.Body;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> GetCommentForUserAsync(int userId, int commentId)
        {
            return await _context.Comments
                           .Where(p => p.UserId == userId && p.Id == commentId)
                           .FirstOrDefaultAsync();
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            return await _context.Comments.AnyAsync(c => c.Id == commentId);
        }
    }
}
