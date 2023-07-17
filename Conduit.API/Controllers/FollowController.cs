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
    [Route("api/Follows")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IFollowsRepository _followsRepository;
        private readonly IMapper _mapper;
        public FollowController(IUserRepository userRepository, IMapper mapper, IFollowsRepository followsRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _followsRepository = followsRepository ?? throw new ArgumentNullException(nameof(followsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("Followers")]
        public IActionResult GetFollowers()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var FollowersForUser = _followsRepository.GetFollowersForUser(userId);
            return Ok(FollowersForUser);
        }

        [HttpGet("Followees")]
        public IActionResult GetFollowees()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var FolloweesForUser = _followsRepository.GetFolloweesForUser(userId);
            return Ok(FolloweesForUser);
        }

        [HttpPost()]
        public async Task<ActionResult> FollowUser(int followeeId)
        {
            var currentUser = GetCurrentUser();
            var followerId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _userRepository.UserExistsAsync(followeeId))
            {
                return NotFound("User you want to follow is not exists");
            }
            if(followeeId == followerId)
            {
                return BadRequest($"You can't follow yourself!");
            }
            if (_followsRepository.IsDuplicatedFollowee(followerId, followeeId))
            {
                return BadRequest($"You already followed user with id: {followeeId}!");
            }
            await _followsRepository.FollowUserAsync(followerId, followeeId);
            return Ok($"You followed user with id: {followeeId} succesfully");
        }

        [HttpDelete("Follower")]
        public async Task<ActionResult> DeleteFollower(int followerId)
        {
            var currentUser = GetCurrentUser();
            var currentUserId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _userRepository.UserExistsAsync(followerId))
            {
                return NotFound($"This user with id: {followerId} is not found");
            }

            var isDeleted = await _followsRepository.DeleteFollowerAsync(followerId, currentUserId);
            if (isDeleted)
            {
                return Ok($"Follower with Id: {followerId} was deleted");
            }
            return BadRequest($"user with id: {followerId} don't follow you");
        }

        [HttpDelete("Followee")]
        public async Task<ActionResult> DeleteFollowee(int followeeId)
        {
            var currentUser = GetCurrentUser();
            var currentUserId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _userRepository.UserExistsAsync(followeeId))
            {
                return NotFound($"This user with id: {followeeId} is not found");
            }

            var isDeleted = await _followsRepository.DeleteFolloweeAsync(followeeId, currentUserId);
            if (isDeleted)
            {
                return Ok($"Followee with Id: {followeeId} was deleted");
            }
            return BadRequest($"You don't follow user with id: {followeeId}");
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
