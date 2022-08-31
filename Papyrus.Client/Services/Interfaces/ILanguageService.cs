using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Interfaces;

public interface ILanguageService : IHttpCall<int>
{
    Task<LanguageDTO?> GetUserLanguage();
    Task SetUserLanguage(int id);
}
