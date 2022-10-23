namespace Papyrus.Shared.DTOs.Groups;

public class GroupNoteListDTO
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime ContentLastEdit { get; set; }
}
