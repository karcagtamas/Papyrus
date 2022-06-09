using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;
using Papyrus.DataAccess.Entities.Notes;

namespace Papyrus.DataAccess.Entities.Groups;

public class Group : IEntity<int>, ICreationEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public DateTime Creation { get; set; }

    [User(onlyInit: true)]
    [Required]
    public string OwnerId { get; set; } = default!;

    public virtual User Owner { get; set; } = default!;
    public virtual ICollection<GroupMember> Members { get; set; } = default!;
    public virtual ICollection<GroupRole> Roles { get; set; } = default!;
    public virtual ICollection<GroupActionLog> ActionLogs { get; set; } = default!;
    public virtual ICollection<Note> Notes { get; set; } = default!;
    public virtual ICollection<Tag> Tags { get; set; } = default!;
}
