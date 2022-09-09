using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupRoleService : IHttpCall<int>
{
    Task<List<GroupRoleDTO>> GetTranslatedByGroup(int groupId, string? textFilter = null, string? lang = null);
    Task<List<GroupRoleLightDTO>> GetLightTranslatedByGroup(int groupId, string? lang = null);
    Task<GroupRoleLightDTO?> GetLightTranslated(int id, string? lang = null);
    Task<bool> Exists(int groupId, string name);
}
