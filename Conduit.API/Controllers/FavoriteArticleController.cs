using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
using Conduit.Db.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Conduit.API.Controllers
{
    [Authorize]
    [Route("api/FavoriteArticle")]
    [ApiController]
    public class FavoriteArticleController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IFavoriteArticleRepository _favoriteArticleRepository;
        private IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public FavoriteArticleController(IUserRepository userRepository, IMapper mapper, IFavoriteArticleRepository favoriteArticleRepository, IArticleRepository articleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _favoriteArticleRepository = favoriteArticleRepository ?? throw new ArgumentNullException(nameof(favoriteArticleRepository));
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetAllFavoriteArticlesForUser()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var FavoriteArticlesForUser = _favoriteArticleRepository.GetAllFavoriteArticlesForUserAsync(userId);
            return Ok(FavoriteArticlesForUser);
        }

        [HttpPost]
        public async Task<ActionResult> AddFavoriteArticle(int articleId)
        {
            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound();
            }
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);
            if (_favoriteArticleRepository.IsDuplicatedFavoriteArticle(userId, articleId))
            {
                return BadRequest($"You already favorite article with id: {articleId}!");
            }
            await _favoriteArticleRepository.AddFavoriteArticleAsync(articleId, userId);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFavoriteArticle(int articleId)
        {
            if (!await _articleRepository.ArticleExistsAsync(articleId))
            {
                return NotFound($"This article with id: {articleId} is not found");
            }
            if (!await _favoriteArticleRepository.FavoriteArticleExistsAsync(articleId))
            {
                return NotFound($"you didn't favorite this artcile yet!");
            }
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            await _favoriteArticleRepository.DeleteFavoriteArticleAsync(userId, articleId);

            return Ok($"FavoriteArticle with articleId: {articleId} was deleted");
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
