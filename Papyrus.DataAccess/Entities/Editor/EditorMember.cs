using KarcagS.Common.Tools.Entities;
using Papyrus.DataAccess.Entities.Notes;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities.Editor;

public class EditorMember : IEntity<string>
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    public string NoteId { get; set; } = default!;

    [Required]
    public string ConnectionId { get; set; } = default!;

    [Required]
    public DateTime Date { get; set; } = default!;

    public virtual User User { get; set; } = default!;
    public virtual Note Note { get; set; } = default!;
}
