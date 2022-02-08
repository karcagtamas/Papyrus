using System.ComponentModel.DataAnnotations;
using Karcags.Common.Tools.Entities;

namespace NoteWeb.DataAccess.Entities;

public class NoteActionLog : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int NoteId { get; set; }

    public virtual Note Note { get; set; }
}
