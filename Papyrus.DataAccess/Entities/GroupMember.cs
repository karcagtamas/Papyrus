using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class GroupMember : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    public int GroupId { get; set; }

    [Required]
    public int RoleId { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;
    public virtual GroupRole Role { get; set; } = default!;
}
