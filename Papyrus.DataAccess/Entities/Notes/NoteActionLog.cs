using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;
using KarcagS.Shared.Common;
using Papyrus.Shared.Enums.Notes;

namespace Papyrus.DataAccess.Entities.Notes;

public class NoteActionLog : IEntity<long>, IIdentified<long>, ICreationEntity
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    public string NoteId { get; set; } = default!;

    [Required]
    public DateTime Creation { get; set; }

    [Required]
    public NoteActionLogType Type { get; set; }

    [Required]
    public string Text { get; set; } = default!;

    public string? PerformerId { get; set; }

    public virtual Note Note { get; set; } = default!;
    public virtual User? Performer { get; set; }
}
