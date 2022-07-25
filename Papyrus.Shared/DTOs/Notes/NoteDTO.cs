namespace Papyrus.Shared.DTOs.Notes;

public class NoteDTO : NoteLightDTO
{
    public string Content { get; set; } = default!;
    public string? LastUpdater { get; set; }
    public DateTime Creation { get; set; }
}
