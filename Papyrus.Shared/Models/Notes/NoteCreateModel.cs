using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Notes;

public class NoteCreateModel
{
    public int? GroupId { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public string FolderId { get; set; } = default!;
}
