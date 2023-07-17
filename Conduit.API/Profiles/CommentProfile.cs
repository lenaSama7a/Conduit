using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
namespace Conduit.API.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<CommentForCreationDto, Comment>();
            CreateMap<Comment, CommentForCreationDto>();
            CreateMap<CommentForUpdateDto, Comment>();
            CreateMap<CommentForUpdateDto, CommentDto>();
            CreateMap<Comment, CommentForUpdateDto>();
        }
    }
}
