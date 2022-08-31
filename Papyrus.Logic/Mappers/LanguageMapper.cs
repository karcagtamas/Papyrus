using AutoMapper;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Mappers;

public class LanguageMapper : Profile
{
    public LanguageMapper()
    {
        CreateMap<Language, LanguageDTO>();
    }
}
