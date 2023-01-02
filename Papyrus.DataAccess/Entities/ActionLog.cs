using KarcagS.Common.Tools.Entities;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities;

public class ActionLog : IEntity<long>
{
    [Required]
    public long Id { get; set; }

    [Required]
    public string Key { get; set; } = default!;

    [Required]
    public string Segment { get; set; } = default!;

    [Required]
    public string Language { get; set; } = default!;

    [Required]
    public string Type { get; set; } = default!;

    [Required]
    public string Text { get; set; } = default!;

    [Required]
    public DateTime Creation { get; set; }

    public string? PerformerId { get; set; }

    public virtual User? Performer { get; set; }
}
