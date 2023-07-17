using AutoMapper;
using Conduit.Db.Entities;
using Conduit.API.Models;
namespace Conduit.API.Profiles
{
    public class FavoriteArticleProfile : Profile
    {
        public FavoriteArticleProfile()
        {
            CreateMap<FavoriteArticle, FavoriteArticleDto>();
            CreateMap<FavoriteArticleDto, FavoriteArticle>();
        }

    }
}
