using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities.Notes;

public class NoteTag
{
    [Required]
    public string NoteId { get; set; } = default!;

    [Required]
    public int TagId { get; set; }

    public virtual Note Note { get; set; } = default!;

    public virtual Tag Tag { get; set; } = default!;
}
