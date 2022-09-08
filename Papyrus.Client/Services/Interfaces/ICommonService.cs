using Microsoft.AspNetCore.Components.Authorization;

namespace Papyrus.Client.Services.Interfaces;

public interface ICommonService
{
    string GetFileUrl(string id);
    Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles);
    bool IsInRole(AuthenticationState state, params string[] roles);
}
