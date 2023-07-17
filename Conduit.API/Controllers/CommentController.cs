using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
using Conduit.Db.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Conduit.API.Controllers
{
    [Authorize]
    [Route("api/Comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ICommentRepository _commentRepository;
        private IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public CommentController(IUserRepository userRepository, IMapper mapper, ICommentRepository commentRepository, IArticleRepository articleRepository )
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("Article/articleId")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllCommentsOfArticle(int articleId)
        {
            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound("Article is not found");
            }

            var CommentsForArticle = await _commentRepository.GetCommentsForArticleAsync(articleId);

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(CommentsForArticle));
        }

        [HttpGet("{commentId}", Name = "GetComment")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComment(int commentId)
        {
            if(! await _commentRepository.CommentExistsAsync(commentId))
            {
                return NotFound($"Comment with id: {commentId} is not found");
            }
            var comment = await _commentRepository.GetCommentAsync(commentId);

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost("Article/{articleId}")]
        public async Task<ActionResult<CommentDto>> CreateComment(int articleId, CommentForCreationDto comment)
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound();
            }

            var Comment = _mapper.Map<Conduit.Db.Entities.Comment>(comment);

            await _commentRepository.AddCommentForArticleAsync(userId, articleId, Comment);

            await _articleRepository.SaveChangesAsync();

            var createdCommentToReturn = _mapper.Map<Models.CommentDto>(Comment);

            return Ok(createdCommentToReturn);
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult> UpdateComment(int commentId, CommentForUpdateDto comment)
        {
            var currentUser = GetCurrentUser();
            var userIdForCurrentUser = _userRepository.GetIdByUserName(currentUser.UserName);
            if (!await _commentRepository.CommentExistsAsync(commentId))
            {
                return NotFound($"Comment with id: {commentId} is not found");
            }
            var userIdForComment = _commentRepository.getUserIdForComment(commentId);
            if (userIdForCurrentUser != userIdForComment)
            {
                return BadRequest("you can't update someone else's comment");
            }
            var commentToUpdate = _mapper.Map<CommentDto>(comment);
            commentToUpdate.Id = commentId;
            var commentAfterUpdate = await _commentRepository
                .UpdateCommentAsync(_mapper.Map<Conduit.Db.Entities.Comment>(commentToUpdate));
            return Ok(_mapper.Map<CommentDto>(commentAfterUpdate));
        }

        [HttpDelete(("{commentId}"))]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            var currentUser = GetCurrentUser();
            var userIdForCurrentUser = _userRepository.GetIdByUserName(currentUser.UserName);
            if (!await _commentRepository.CommentExistsAsync(commentId))
            {
                return NotFound($"Comment with Id: {commentId} is not exist");
            }
            var userIdForComment = _commentRepository.getUserIdForComment(commentId);
            if (userIdForCurrentUser != userIdForComment)
            {
                return BadRequest("You can't delete someone else's comment");
            }
            var commentEntity = await _commentRepository.GetCommentForUserAsync(userIdForComment, commentId);
            _commentRepository.DeleteComment(commentEntity);
            await _userRepository.SaveChangesAsync();

            return Ok($"Comment with Id: {commentId} was deleted");

        }
        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                };
            }
            return null;
        }
    }
}
