using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
using Conduit.Db.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using FluentValidation.Results;
using Conduit.API.Validators;
using FluentValidation;

namespace Conduit.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var userEntities = await _userRepository.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(userEntities));
        }

        [HttpGet("searching")]
        [Authorize]
        public async Task<IActionResult> GetUsersByName(string? name, bool includeArticles = true)
        {
            var user = await _userRepository.GetUsersByNameAsync(name, includeArticles);
            if (user == null)
            {
                return NotFound("This user is not exist");
            }
            if (includeArticles)
            {
                return Ok(_mapper.Map<IEnumerable<UserWithArticlesDto>>(user));
            }
            return Ok(_mapper.Map<IEnumerable<UserDto>>(user));
        }

        [HttpGet("HomePage")]
        [Authorize]
        public IActionResult GetArticlesOfFollowees()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var Articles = _userRepository.GetArticlesofFollowees(userId);
            return Ok(Articles);
        }

        [HttpGet("{userId}", Name = "GetUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserByIdAsync(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByIdAsync(userId, false);
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserForCreationDto user)
        {
            if (!await _userRepository.ValidateuserName(user.UserName))
            {
                return BadRequest("This user name is already in use");
            }

            var userToCreate = _mapper.Map<Conduit.Db.Entities.User>(user);
            var validator = new UserValidator();
            ValidationResult result = validator.Validate(userToCreate);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            await _userRepository.CreateUserAsync(userToCreate);
            var createdUserToReturn = _mapper.Map<Models.UserDto>(userToCreate);

            return CreatedAtRoute("GetUser",
                 new
                 {
                     userId = createdUserToReturn.Id
                 },
                 createdUserToReturn);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateCurrentUser( UserForUpdateDto user)
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            var userToUpdate = _mapper.Map<UserWithPasswordDto>(user);
            var validator = new UserValidator();
            var userForUpdate = _mapper.Map<User>(userToUpdate);
            ValidationResult result = validator.Validate(userForUpdate);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            userToUpdate.Id = userId;
            userToUpdate.UserName = currentUser.UserName;

            var userAfterUpdate = await _userRepository.UpdateUserAsync(_mapper.Map<Conduit.Db.Entities.User>(userToUpdate));

            return Ok(_mapper.Map<UserDto>(userAfterUpdate));
        }

        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> PartiallyUpdateUser([FromBody] JsonPatchDocument<UserForUpdateDto> patchDocument)
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);
            var user = await _userRepository.GetUserByIdAsync(userId, false);
            if (user == null)
            {
                return NotFound();
            }
            var validator = new UserValidator();
            var userForUpdate = _mapper.Map<User>(user);
            ValidationResult result = validator.Validate(userForUpdate);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }

            var userToPatch = _mapper.Map<UserForUpdateDto>(user);

            patchDocument.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(userToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(userToPatch, user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            var currentUser = GetCurrentUser();
            var userId = _userRepository.GetIdByUserName(currentUser.UserName);

            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"This user with id: {userId} is not found");
            }

            await _userRepository.DeleteUserAsync(userId);

            return Ok($"User with Id: {userId} was deleted");
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
