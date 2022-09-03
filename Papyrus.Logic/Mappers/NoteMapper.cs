using AutoMapper;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Mappers;

public class NoteMapper : Profile
{
    public NoteMapper()
    {
        CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Creator, x => x.UserName)))
            .ForMember(dest => dest.LastUpdater, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.LastUpdater, x => x.UserName)))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()));
        CreateMap<Note, NoteLightDTO>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Creator, x => x.UserName)))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Tag).ToList()));

        CreateMap<NoteModel, Note>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore());

        CreateMap<NoteTag, NoteTagDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Tag.Id))
            .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Tag.Caption))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Tag.Color));
        CreateMap<Tag, NoteTagDTO>();
    }
}
