namespace Papyrus.Shared.DTOs.External;

public class GroupListExtDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime Creation { get; set; }
    public string Url { get; set; } = default!;
}
