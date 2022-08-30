using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupMemberService : IHttpCall<int>
{
    Task<List<GroupMemberDTO>> GetByGroup(int groupId, string? textFilter = null);
    Task<List<string>> GetMemberKeys(List<int> memberIds);
}
