using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;

namespace Papyrus.DataAccess.Entities;

public class User : IdentityUser<string>, IEntity<string>
{
    public string OsirisId { get; set; }

    [Required]
    public DateTime Registration { get; set; }

    [Required]
    public DateTime LastLogin { get; set; }

    [MaxLength(100)]
    public string FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    [Required]
    public bool Disabled { get; set; }

    public string ImageTitle { get; set; }

    public byte[] ImageData { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Group> CreatedGroups { get; set; }
    public virtual ICollection<GroupMember> Groups { get; set; }
    public virtual ICollection<Note> Notes { get; set; }
}
