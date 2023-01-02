using KarcagS.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.DataAccess.Entities.Profile;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities;

public class User : IdentityUser<string>, IEntity<string>
{
    public string? OsirisId { get; set; }

    public DateTime? LastOsirisSync { get; set; }

    [Required]
    public DateTime Registration { get; set; }

    [Required]
    public DateTime LastLogin { get; set; }

    [MaxLength(100)]
    public string? FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    [Required]
    public bool Disabled { get; set; }

    public string? ImageId { get; set; }

    public int? LanguageId { get; set; }

    [Required]
    public int Theme { get; set; }

    public bool IsOsirisConnected { get { return OsirisId != null; } }

    public virtual Language? Language { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<Group> CreatedGroups { get; set; } = default!;
    public virtual ICollection<GroupMember> Groups { get; set; } = default!;
    public virtual ICollection<Note> Notes { get; set; } = default!;
    public virtual ICollection<GroupMember> AddedGroupMembers { get; set; } = default!;
    public virtual ICollection<Tag> Tags { get; set; } = default!;
    public virtual ICollection<Note> CreatedNotes { get; set; } = default!;
    public virtual ICollection<Note> LastUpdatedNotes { get; set; } = default!;
    public virtual ICollection<EditorMember> EditorMemberships { get; set; } = default!;
    public virtual ICollection<ActionLog> ActionLogs { get; set; } = default!;
    public virtual ICollection<Folder> CreatedFolders { get; set; } = default!;
    public virtual ICollection<Folder> Folders { get; set; } = default!;

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = default!;

    public virtual ICollection<Post> CreatedPosts { get; set; } = default!;
    public virtual ICollection<Post> UpdatedPosts { get; set; } = default!;
    public virtual ICollection<AppAccess> AppAccesses { get; set; } = default!;
    public virtual ICollection<NoteAccess> NoteAccesses { get; set; } = default!;
    public virtual ICollection<Application> Applications { get; set; } = default!;
}
