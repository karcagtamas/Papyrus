namespace Papyrus.Shared.DTOs.Notes;

public class FolderDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime Creation { get; set; }
    public DateTime LastUpdate { get; set; }
}
