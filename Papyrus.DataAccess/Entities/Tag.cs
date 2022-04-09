using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class Tag : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Caption { get; set; } = default!;

    public string Description { get; set; } = default!;

    [Required]
    public string Color { get; set; } = default!;

    public int? GroupId { get; set; }

    public int? ParentId { get; set; }

    public virtual Group Group { get; set; } = default!;
    public virtual Tag Parent { get; set; } = default!;
    public virtual ICollection<Tag> Children { get; set; } = default!;
}
