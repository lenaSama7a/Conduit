using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
using System.Numerics;

namespace Conduit.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserForUpdateDto>();
            CreateMap< UserForUpdateDto, User>();
            CreateMap<User, UserWithArticlesDto>();
            CreateMap<UserForCreationDto, UserDto>();
            CreateMap<UserForCreationDto, User>();
            CreateMap<UserForUpdateDto, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserForUpdateDto, UserWithPasswordDto>();
            CreateMap<UserWithPasswordDto, User>();
        }

    }
}
