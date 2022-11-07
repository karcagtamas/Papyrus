using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.Models;

namespace Papyrus.Logic.Mappers;

public class PostMapper : AutoMapper.Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostDTO>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Creator, u => u.UserName) ?? "N/A"))
            .ForMember(dest => dest.LastUpdater, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.LastUpdater, u => u.UserName) ?? "N/A"));
        CreateMap<PostModel, Post>();
    }
}
