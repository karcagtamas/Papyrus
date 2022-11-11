using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Utils;
using Papyrus.Shared.DTOs.External;

namespace Papyrus.Logic.Mappers.External;

public class ExternalMapper : AutoMapper.Profile
{
    public ExternalMapper()
    {
        CreateMap<Note, NoteExtDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()))
            .ForMember(dest => dest.Url, opt => opt.Ignore());
        CreateMap<Note, NoteContentExtDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructApiUrl($"Notes/{src.Id}")))
            .ForMember(dest => dest.Content, opt => opt.Ignore());
        CreateMap<Tag, TagExtDTO>();
        CreateMap<Tag, TagTreeExtDTO>()
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children.ToList()));
        CreateMap<Group, GroupListExtDTO>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructGroupUrl(src.Id, null)));
        CreateMap<Group, GroupExtDTO>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructGroupUrl(src.Id, null)))
            .ForMember(dest => dest.NotesUrl, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructGroupUrl(src.Id, "Notes")))
            .ForMember(dest => dest.TagsUrl, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructGroupUrl(src.Id, "Tags")))
            .ForMember(dest => dest.MembersUrl, opt => opt.MapFrom(src => ExternalUrlHelper.ConstructGroupUrl(src.Id, "Members")));
        CreateMap<GroupMember, GroupMemberExtDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
    }
}
