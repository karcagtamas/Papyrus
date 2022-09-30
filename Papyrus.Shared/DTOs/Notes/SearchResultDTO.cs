namespace Papyrus.Shared.DTOs.Notes;

public class SearchResultDTO
{
    public string NoteId { get; set; } = default!;
    public string DisplayTitle { get; set; } = default!;
    public bool PublicStatus { get; set; } = true;
    public List<NoteTagDTO> Tags { get; set; } = new();
    public DateTime Creation { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool Archived { get; set; } = false;
    public SearchResultDataDTO Data { get; set; } = new();
}
