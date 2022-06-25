namespace Papyrus.Shared.DTOs.Notes;

public class TagDTO
{
    public int Id { get; set; }
    public string Caption { get; set; } = default!;
    public string? Description { get; set; }
    public string Color { get; set; } = default!;
    public int? ParentId { get; set; }
}
