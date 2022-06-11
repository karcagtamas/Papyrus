using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupRoleService : IHttpCall<int>
{
    Task<List<GroupRoleDTO>> GetGroupList(int groupId);
}
