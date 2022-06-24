using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupRoleService : IHttpCall<int>
{
    Task<List<GroupRoleDTO>> GetByGroup(int groupId);
    Task<List<GroupRoleLightDTO>> GetLightByGroup(int groupId);
    Task<bool> Exists(int groupId, string name);
}
