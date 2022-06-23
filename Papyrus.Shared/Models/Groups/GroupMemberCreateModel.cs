using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupMemberCreateModel
{
    [Required]
    public int GroupId { get; set; }

    [Required]
    public string UserId { get; set; } = default!;
}
