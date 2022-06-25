namespace Papyrus.Shared.DTOs.Groups;

public class GroupTagDTO
{
    public int Id { get; set; }
    public string Caption { get; set; } = default!;
    public string? Description { get; set; }
    public string Color { get; set; } = default!;
}
