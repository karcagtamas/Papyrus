using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.Entities;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.DataAccess.Entities.Groups;

namespace Papyrus.DataAccess.Entities.Notes;

public class Note : IEntity<string>, ICreationEntity, ILastUpdateEntity
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    [MaxLength(40)]
    public string Title { get; set; } = default!;

    [Required]
    public bool Public { get; set; }

    [Required]
    public bool Archived { get; set; }

    [Required]
    public DateTime Creation { get; set; }

    [Required]
    public DateTime LastUpdate { get; set; }

    [User(onlyInit: true)]
    public string? CreatorId { get; set; }

    [User]
    public string? LastUpdaterId { get; set; }

    [Required]
    public string ContentId { get; set; } = default!;

    [Required]
    public string FolderId { get; set; } = default!;

    public DateTime? ContentLastEdit { get; set; }
    public string? UserId { get; set; }
    public int? GroupId { get; set; }

    public virtual User? Creator { get; set; }
    public virtual User? LastUpdater { get; set; }
    public virtual Folder Folder { get; set; } = default!;
    public virtual User? User { get; set; }
    public virtual Group? Group { get; set; }
    public virtual ICollection<NoteTag> Tags { get; set; } = default!;
    public virtual ICollection<EditorMember> EditorMemberships { get; set; } = default!;
}
