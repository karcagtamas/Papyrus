using System.ComponentModel.DataAnnotations;
using KarcagS.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class Note : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public int? GroupId { get; set; }

    [Required]
    public bool Public { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;
    public virtual ICollection<NoteActionLog> ActionLogs { get; set; } = default!;
}
