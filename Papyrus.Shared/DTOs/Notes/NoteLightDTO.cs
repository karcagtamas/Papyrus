namespace Papyrus.Shared.DTOs.Notes;

public class NoteLightDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime LastUpdate { get; set; }
    public string? Creator { get; set; }
    public bool Public { get; set; }
    public List<NoteTagDTO> Tags { get; set; } = default!;
    public int? GroupId { get; set; }
}
