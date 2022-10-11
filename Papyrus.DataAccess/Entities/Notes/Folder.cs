using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;
using Papyrus.DataAccess.Entities.Groups;

namespace Papyrus.DataAccess.Entities.Notes;

public class Folder : IEntity<string>, ICreationEntity, ILastUpdateEntity
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    public string? ParentId { get; set; }

    [Required]
    [MaxLength(40)]
    public string Title { get; set; } = default!;

    [Required]
    public DateTime Creation { get; set; }

    [Required]
    public DateTime LastUpdate { get; set; }

    public string? UserId { get; set; }
    public int? GroupId { get; set; }

    [User(onlyInit: true)]
    public string? CreatorId { get; set; }

    public virtual Folder? Parent { get; set; }
    public virtual User? User { get; set; }
    public virtual Group? Group { get; set; }
    public virtual User? Creator { get; set; }
    public virtual ICollection<Note> Notes { get; set; } = default!;
    public virtual ICollection<Folder> Folders { get; set; } = default!;
}
