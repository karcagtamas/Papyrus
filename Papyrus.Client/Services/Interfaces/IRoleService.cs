using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs;

namespace Papyrus.Client.Services.Interfaces;

public interface IRoleService : IHttpCall<string>
{
    Task<List<RoleDTO>> GetAllTranslated(string? lang = null);
}
