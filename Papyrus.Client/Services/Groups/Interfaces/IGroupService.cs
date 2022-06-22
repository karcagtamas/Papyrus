using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupService : IHttpCall<int>
{
    Task<List<GroupListDTO>> GetUserList();
    Task<List<GroupMemberDTO>> GetMembers(int groupId);
    Task<bool> AddMember(int groupId, string memberId);
    Task<bool> RemoveMember(int groupId, int groupMemberId);
    Task<bool> EditMember(int groupId, int groupMemberId, GroupMemberModel model);
}
