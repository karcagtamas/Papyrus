namespace Papyrus.Shared.DTOs.Groups;

public class GroupMemberDTO
{
    public int Id { get; set; }
    public UserLightDTO User { get; set; } = default!;
    public GroupRoleLightDTO Role { get; set; } = default!;
    public UserLightDTO? AddedBy { get; set; }
    public DateTime Join { get; set; }
}
