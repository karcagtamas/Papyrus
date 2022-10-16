namespace Papyrus.Shared.DTOs.Notes;

public class NoteLightDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime LastUpdate { get; set; }
    public DateTime Creation { get; set; }
    public string? Creator { get; set; }
    public string? LastUpdater { get; set; }
    public bool Public { get; set; }
    public bool Archived { get; set; }
    public List<NoteTagDTO> Tags { get; set; } = default!;
    public int? GroupId { get; set; }
    public string ContentId { get; set; } = default!;
    public DateTime ContentLastEdit { get; set; }
    public string FolderId { get; set; } = default!;
}
