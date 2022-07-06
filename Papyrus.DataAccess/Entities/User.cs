using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.DataAccess.Entities.Notes;

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

    public string? ImageTitle { get; set; }

    public byte[]? ImageData { get; set; }

    public bool IsOsirisConnected { get { return OsirisId != null; } }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<Group> CreatedGroups { get; set; } = default!;
    public virtual ICollection<GroupMember> Groups { get; set; } = default!;
    public virtual ICollection<Note> Notes { get; set; } = default!;
    public virtual ICollection<GroupMember> AddedGroupMembers { get; set; } = default!;
    public virtual ICollection<Tag> Tags { get; set; } = default!;
    public virtual ICollection<GroupActionLog> PerformedGroupActionLogs { get; set; } = default!;
}
