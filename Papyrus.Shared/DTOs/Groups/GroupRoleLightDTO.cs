using KarcagS.Shared.Common;

namespace Papyrus.Shared.DTOs.Groups;

public class GroupRoleLightDTO : IIdentified<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
