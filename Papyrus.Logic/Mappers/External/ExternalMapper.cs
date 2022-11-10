using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.External;

namespace Papyrus.Logic.Mappers.External;

public class ExternalMapper : AutoMapper.Profile
{
    public ExternalMapper()
    {
        CreateMap<Note, NoteExtDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ConstructApiUrl($"Notes/{src.Id}")));
        CreateMap<Note, NoteContentExtDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ConstructApiUrl($"Notes/{src.Id}")))
            .ForMember(dest => dest.Content, opt => opt.Ignore());
        CreateMap<Tag, TagExtDTO>();
        CreateMap<Tag, TagTreeExtDTO>()
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children.ToList()));
        CreateMap<Group, GroupListExtDTO>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ConstructGroupUrl(src.Id, null)));
        CreateMap<Group, GroupExtDTO>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ConstructGroupUrl(src.Id, null)))
            .ForMember(dest => dest.NotesUrl, opt => opt.MapFrom(src => ConstructGroupUrl(src.Id, "Notes")))
            .ForMember(dest => dest.TagsUrl, opt => opt.MapFrom(src => ConstructGroupUrl(src.Id, "Tags")))
            .ForMember(dest => dest.MembersUrl, opt => opt.MapFrom(src => ConstructGroupUrl(src.Id, "Members")));
    }

    private static string ConstructGroupUrl(int id, string? path = null) => ConstructApiUrl(String.Join("/", new List<string?> { "Groups", id.ToString(), path }.Where(x => ObjectHelper.IsNotNull(x)).ToList()));

    private static string ConstructApiUrl(string path) => $"/api/External/{path}";
}
