using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace NoteWeb.DataAccess.Entities;

public class Tag : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Caption { get; set; }

    public string Description { get; set; }

    [Required]
    public string Color { get; set; }

    public int? GroupId { get; set; }

    public int? ParentId { get; set; }

    public virtual Group Group { get; set; }
    public virtual Tag Parent { get; set; }
    public virtual ICollection<Tag> Children { get; set; }
}
