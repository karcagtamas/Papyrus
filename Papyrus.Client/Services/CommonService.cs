using KarcagS.Blazor.Common.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Papyrus.Client.Services.Interfaces;

namespace Papyrus.Client.Services;

public class CommonService : ICommonService
{
    public string GetFileUrl(string id) => $"{ApplicationSettings.BaseApiUrl}/File/{id}";

    public async Task<bool> IsInRole(Task<AuthenticationState> stateTask, params string[] roles) => IsInRole(await stateTask, roles);

    public bool IsInRole(AuthenticationState state, params string[] roles) => roles.ToList().Any(r => state.User.IsInRole(r));
}
