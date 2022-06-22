using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupMemberModel
{
    [Required]
    public int RoleId { get; set; } = default!;
}
