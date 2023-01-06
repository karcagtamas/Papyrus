using KarcagS.Common.Tools.Entities;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities.Notes;

public class NoteAccess : IEntity<string>
{

    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    public string NoteId { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual Note Note { get; set; } = default!;
}
