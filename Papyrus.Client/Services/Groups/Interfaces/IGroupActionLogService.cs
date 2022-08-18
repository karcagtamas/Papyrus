using KarcagS.Blazor.Common.Http;
using KarcagS.Shared.Table;
using Papyrus.Shared.DTOs.Groups.ActionsLogs;

namespace Papyrus.Client.Services.Groups.Interfaces;

public interface IGroupActionLogService : IHttpCall<long>
{
    Task<TableResult<GroupActionLogDTO>> GetByGroup(int groupId, int? page = null, int? size = null);
}
