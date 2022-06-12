using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities.Groups;

public class GroupMember : IEntity<int>, ICreationEntity
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

    [Required]
    public string? AddedById { get; set; }

    [Required]
    public DateTime Creation { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;
    public virtual GroupRole Role { get; set; } = default!;
    public virtual User? AddedBy { get; set; } = default!;
}
