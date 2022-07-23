using AutoMapper;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Mappers;

public class NoteMapper : Profile
{
    public NoteMapper()
    {
        CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Creator, x => x.UserName)))
            .ForMember(dest => dest.LastUpdater, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.LastUpdater, x => x.UserName)));
        CreateMap<Note, NoteLightDTO>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ObjectHelper.MapOrDefault(src.Creator, x => x.UserName)));
    }
}
