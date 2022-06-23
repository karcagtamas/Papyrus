using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupService : IHttpCall<int>
{
    Task<List<GroupListDTO>> GetUserList();
    Task<bool> IsClosable(int id);
    Task<bool> Close(int id);
}
