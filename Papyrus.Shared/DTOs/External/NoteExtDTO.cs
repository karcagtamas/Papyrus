namespace Papyrus.Shared.DTOs.External;

public class NoteExtDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime LastUpdate { get; set; }
    public DateTime Creation { get; set; }
    public bool Public { get; set; }
    public bool Archived { get; set; }
    public string Url { get; set; } = default!;
    public List<TagExtDTO> Tags { get; set; } = default!;
}
