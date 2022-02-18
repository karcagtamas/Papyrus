using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = default!;

    [Required]
    public DateTime Expires { get; set; }

    [Required]
    public DateTime Created { get; set; }

    public DateTime? Revoked { get; set; }

    [Required]
    public string UserId { get; set; } = default!;
    public string ClientId { get; set; } = default!;

    public virtual User User { get; set; } = default!;


    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
}
