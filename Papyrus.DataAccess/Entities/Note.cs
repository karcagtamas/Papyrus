using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace Papyrus.DataAccess.Entities;

public class Note : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public string UserId { get; set; }
    public int? GroupId { get; set; }

    [Required]
    public bool Public { get; set; }

    public virtual User User { get; set; }
    public virtual Group Group { get; set; }
    public virtual ICollection<NoteActionLog> ActionLogs { get; set; }
}
