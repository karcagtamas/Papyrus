using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;

namespace NoteWeb.DataAccess.Entities;

public class User : IdentityUser<string>, IEntity<string>
{
    [Required]
    public string OsirisId { get; set; }

    [Required]
    public DateTime Registration { get; set; }

    [Required]
    public DateTime LastLogin { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
