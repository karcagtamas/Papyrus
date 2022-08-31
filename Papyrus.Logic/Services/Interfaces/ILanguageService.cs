using KarcagS.Common.Tools.Repository;
using Papyrus.DataAccess.Entities;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services.Interfaces;

public interface ILanguageService : IMapperRepository<Language, int>
{
    LanguageDTO? GetUserLanguage();
    void SetUserLanguage(int id);
}
