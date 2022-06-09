using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupMemberModel
{
    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    public int RoleId { get; set; } = default!;
}
