namespace Papyrus.Shared.Models.Groups;

public class GroupTagModel
{
    public int GroupId { get; set; }
    public string Caption { get; set; } = default!;
    public string? Description { get; set; }
    public string Color { get; set; } = default!;
}
