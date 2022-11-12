using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Mappers;

public class LanguageMapper : AutoMapper.Profile
{
    public LanguageMapper()
    {
        CreateMap<Language, LanguageDTO>();
    }
}
