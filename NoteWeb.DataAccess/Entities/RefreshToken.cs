using System.ComponentModel.DataAnnotations;

namespace NoteWeb.DataAccess.Entities;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public DateTime Expires { get; set; }

    [Required]
    public DateTime Created { get; set; }

    public DateTime? Revoked { get; set; }

    [Required]
    public string UserId { get; set; }
    public string ClientId { get; set; }

    public virtual User User { get; set; }


    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
}
