namespace Papyrus.Shared.DTOs.Notes;

public class NoteDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? Creator { get; set; }
    public string? LastUpdater { get; set; }
    public DateTime Creation { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool Public { get; set; }
}
