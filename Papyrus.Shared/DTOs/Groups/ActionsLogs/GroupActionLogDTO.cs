using Papyrus.Shared.Enums.Groups;
using KarcagS.Shared.Common;

namespace Papyrus.Shared.DTOs.Groups.ActionsLogs;

public class GroupActionLogDTO : IIdentified<long>
{
    public long Id { get; set; }
    public DateTime Creation { get; set; }
    public GroupActionLogType Type { get; set; }
    public string Text { get; set; } = default!;
    public string? Performer { get; set; }
}
