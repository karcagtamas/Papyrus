namespace Papyrus.Shared.DTOs.Groups;

public class GroupListDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime Creation { get; set; }
    public string Owner { get; set; } = default!;
}
