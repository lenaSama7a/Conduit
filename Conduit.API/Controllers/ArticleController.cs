using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
using Conduit.Db.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.ComponentModel.Design;

namespace Conduit.API.Controllers
{
    [Authorize]
    [Route("api/Articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public ArticleController(IUserRepository userRepository, IMapper mapper, IArticleRepository articleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"user with Id: {userId} is not found");
            }

            var ArticlesForUser = await _articleRepository.GetArticlesForUserAsync(userId);

            return Ok(_mapper.Map<IEnumerable<ArticleDto>>(ArticlesForUser));
        }

        [HttpGet("MyArticles")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetMyArticles()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var ArticlesForUser = await _articleRepository.GetArticlesForUserAsync(userId);
            return Ok(_mapper.Map<IEnumerable<ArticleDto>>(ArticlesForUser));
        }

        [HttpGet("{articleId}", Name = "GetArticle")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticle(int articleId)
        {
            if(!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound($"Article with id: {articleId} is not exists");

            }
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            return Ok(_mapper.Map<ArticleDto>(article));
        }

        [HttpGet("searching")]
        public async Task<IActionResult> GetArticles(string? tagOrTitle)
        {
            var article = await _articleRepository.GetArticlesByTagAsync(tagOrTitle);
            if (article == null)
            {
                return NotFound($"Article with this tag or title is not exists");
            }
            return Ok(_mapper.Map<IEnumerable<ArticleDto>>(article));
        }

        [HttpPost]
        public async Task<ActionResult<ArticleDto>> CreateArticle(ArticleForCreationDto article)
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var Article = _mapper.Map<Conduit.Db.Entities.Article>(article);

            await _articleRepository.AddArticleForUserAsync(userId, Article);

            await _articleRepository.SaveChangesAsync();

            var createdArticleToReturn = _mapper.Map<Models.ArticleDto>(Article);

            return CreatedAtRoute("GetArticle",
                 new
                 {
                     userId = userId,
                     articleId = createdArticleToReturn.Id
                 },
                 createdArticleToReturn);
        }

        [HttpPut("{articleId}")]
        public async Task<ActionResult> UpdateArticle(int articleId, ArticleForUpdateDto article)
        {
            var currentUser = GetCurrentUser();
            var currentUserId = _userRepository.GetIdByUserName(currentUser.UserName);
            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound();
            }
            var userIdForArticle = _articleRepository.getUserIdForArticle(articleId);

            if (currentUserId != userIdForArticle)
            {
                return BadRequest("you can't update someone else's article");
            }
            var articleToUpdate = _mapper.Map<ArticleDto>(article);
            articleToUpdate.Id = articleId;
            var articleAfterUpdate = await _articleRepository
                .UpdateArticleAsync(_mapper.Map<Conduit.Db.Entities.Article>(articleToUpdate));
            return Ok(_mapper.Map<ArticleDto>(articleAfterUpdate));
        }

        [HttpPatch("{articleId}")]
        public async Task<ActionResult> PartiallyUpdateArticle(int articleId,
            [FromBody] JsonPatchDocument<ArticleForUpdateDto> patchDocument)
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound("This article is not exist");
            }

            var articleEntity = await _articleRepository.GetArticleForUserAsync(userId, articleId);
            if (articleEntity == null)
            {
                return NotFound("you can't edit someone else's Article");
            }

            var articleToPatch = _mapper.Map<ArticleForUpdateDto>(
                articleEntity);

            patchDocument.ApplyTo(articleToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(articleToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(articleToPatch, articleEntity);
            await _articleRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{articleId}")]
        public async Task<ActionResult> DeleteArticle(int articleId)
        {
            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound($"Article with Id: {articleId} is not exists");
            }
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);
            var articleEntity = await _articleRepository.GetArticleForUserAsync(userId, articleId);
            if (articleEntity == null)
            {
                return NotFound("You can't delete this article, it's not your article!!");
            }

            _articleRepository.DeleteArticle(articleEntity);
            await _articleRepository.SaveChangesAsync();

            return Ok($"Article with Id: {articleId} was deleted");
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
