using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class NoteActionLog : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int NoteId { get; set; }

    public virtual Note Note { get; set; } = default!;
}
