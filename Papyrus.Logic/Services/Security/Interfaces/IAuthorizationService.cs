using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Services.Security.Interfaces;

public interface IAuthorizationService
{
    public Task<bool> UserHasApplicationAccessRight(string userId, string appId);
    public Task<bool> UserHasGroupRight(string userId, int groupId, GroupRight right);
}
