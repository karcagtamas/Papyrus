using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupService : IHttpCall<int>
{
    Task<List<GroupListDTO>> GetUserList(bool hideClosed = false);
    Task<GroupRightsDTO> GetRights(int id);
    Task<GroupRoleRightsDTO> GetRoleRights(int id);
    Task<GroupTagRightsDTO> GetTagRights(int id);
    Task<GroupMemberRightsDTO> GetMemberRights(int id);
    Task<bool> Close(int id);
    Task<bool> Open(int id);
    Task<bool> Remove(int id);
}
