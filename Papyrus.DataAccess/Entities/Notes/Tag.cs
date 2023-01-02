using KarcagS.Common.Tools.Entities;
using Papyrus.DataAccess.Entities.Groups;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities.Notes;

public class Tag : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Caption { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    public string Color { get; set; } = default!;

    public int? GroupId { get; set; }

    public string? UserId { get; set; }

    public int? ParentId { get; set; }

    public virtual Group? Group { get; set; }
    public virtual User? User { get; set; }
    public virtual Tag? Parent { get; set; }
    public virtual ICollection<Tag> Children { get; set; } = default!;
    public virtual ICollection<NoteTag> Notes { get; set; } = default!;
}
