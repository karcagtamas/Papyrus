using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupMemberUpdateModel
{
    [Required]
    public int RoleId { get; set; } = default!;
}
