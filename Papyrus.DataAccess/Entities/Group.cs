using System.ComponentModel.DataAnnotations;
using Karcags.Common.Annotations;
using Karcags.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class Group : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public DateTime Creation { get; set; }

    [User]
    [Required]
    public string OwnerId { get; set; } = default!;

    public virtual User Owner { get; set; } = default!;
    public virtual ICollection<GroupMember> Members { get; set; } = default!;
    public virtual ICollection<GroupRole> Roles { get; set; } = default!;
    public virtual ICollection<GroupActionLog> ActionLogs { get; set; } = default!;
    public virtual ICollection<Note> Notes { get; set; } = default!;
    public virtual ICollection<Tag> Tags { get; set; } = default!;
}
