using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;

namespace Papyrus.DataAccess.Entities;

public class User : IdentityUser<string>, IEntity<string>
{
    public string OsirisId { get; set; } = default!;

    [Required]
    public DateTime Registration { get; set; }

    [Required]
    public DateTime LastLogin { get; set; }

    [MaxLength(100)]
    public string FullName { get; set; } = default!;

    public DateTime? BirthDay { get; set; }

    [Required]
    public bool Disabled { get; set; }

    public string ImageTitle { get; set; } = default!;

    public byte[] ImageData { get; set; } = default!;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<Group> CreatedGroups { get; set; } = default!;
    public virtual ICollection<GroupMember> Groups { get; set; } = default!;
    public virtual ICollection<Note> Notes { get; set; } = default!;
}
