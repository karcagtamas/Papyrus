using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities.Notes;

public class NoteActionLog : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string NoteId { get; set; } = default!;

    public virtual Note Note { get; set; } = default!;
}
