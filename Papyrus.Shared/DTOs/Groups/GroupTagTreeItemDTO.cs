namespace Papyrus.Shared.DTOs.Groups;

public class GroupTagTreeItemDTO
{
    public int Id { get; set; } = default!;
    public string Caption { get; set; } = default!;
    public string? Description { get; set; }
    public string Color { get; set; } = default!;
    public List<GroupTagTreeItemDTO> Children { get; set; } = new();
}
