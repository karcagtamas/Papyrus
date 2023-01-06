using KarcagS.Common.Tools.Entities;
using System.ComponentModel.DataAnnotations;
namespace Papyrus.DataAccess.Entities;

public class AppAccess : IEntity<string>
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = default!;
}
