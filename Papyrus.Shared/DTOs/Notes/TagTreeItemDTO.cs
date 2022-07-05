namespace Papyrus.Shared.DTOs.Notes;

public class TagTreeItemDTO
{
    public int Id { get; set; } = default!;
    public string Caption { get; set; } = default!;
    public string? Description { get; set; }
    public string Color { get; set; } = default!;
    public List<TagTreeItemDTO> Children { get; set; } = new();
}
