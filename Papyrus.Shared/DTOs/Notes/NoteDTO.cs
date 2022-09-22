namespace Papyrus.Shared.DTOs.Notes;

public class NoteDTO : NoteLightDTO
{
    public string Content { get; set; } = default!;
    public DateTime Creation { get; set; }
}
