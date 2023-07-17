using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
namespace Conduit.API.Profiles
{
    public class FollowProfile : Profile
    {
        public FollowProfile()
        {
            CreateMap<FollowDto, Follow>();
            CreateMap<Follow, FollowDto>();
        }
    }
}
