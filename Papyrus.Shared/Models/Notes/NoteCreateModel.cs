namespace Papyrus.Shared.Models.Notes;

public class NoteCreateModel
{
    public int? GroupId { get; set; }
    public string Title { get; set; } = default!;
}
