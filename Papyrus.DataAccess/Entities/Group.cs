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
    public string Name { get; set; }

    public DateTime Creation { get; set; }

    [User]
    [Required]
    public string OwnerId { get; set; }

    public virtual User Owner { get; set; }
    public virtual ICollection<GroupMember> Members { get; set; }
    public virtual ICollection<GroupRole> Roles { get; set; }
    public virtual ICollection<GroupActionLog> ActionLogs { get; set; }
    public virtual ICollection<Note> Notes { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
}
