using KarcagS.Shared.Common;

namespace Papyrus.Shared.DTOs.Groups;

public class GroupMemberDTO : IIdentified<int>
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public UserLightDTO User { get; set; } = default!;
    public GroupRoleLightDTO Role { get; set; } = default!;
    public UserLightDTO? AddedBy { get; set; }
    public DateTime Join { get; set; }
}
