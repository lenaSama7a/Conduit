using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
namespace Conduit.API.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();
            CreateMap<ArticleForCreationDto, Article>();
            CreateMap<ArticleForUpdateDto, Article>();
            CreateMap<ArticleForUpdateDto, ArticleDto>();
            CreateMap<Article, ArticleForUpdateDto>();
            CreateMap<ArticleDto, ArticleForUpdateDto>();
        }
    }
}
