using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Groups.Rights;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupService : IHttpCall<int>
{
    void NavigateToBase(int id);
    Task<List<GroupListDTO>> GetUserList(bool hideClosed = false);
    Task<GroupPageRightsDTO> GetPageRights(int id);
    Task<GroupRightsDTO> GetRights(int id);
    Task<GroupRoleRightsDTO> GetRoleRights(int id);
    Task<GroupTagRightsDTO> GetTagRights(int id);
    Task<GroupMemberRightsDTO> GetMemberRights(int id);
    Task<GroupNoteRightsDTO> GetNoteRights(int id);
    Task<bool> Close(int id);
    Task<bool> Open(int id);
    Task<bool> Remove(int id);
}
