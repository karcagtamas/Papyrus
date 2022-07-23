using KarcagS.Blazor.Common.Http;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupActionLogService : IHttpCall<long>
{
    Task<List<GroupActionLogDTO>> GetByGroup(int groupId);
}
