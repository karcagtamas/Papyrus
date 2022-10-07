namespace Papyrus.Shared.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public string Creator { get; set; } = default!;
    public DateTime Creation { get; set; }
    public string LastUpdater { get; set; } = default!;
    public DateTime LastUpdate { get; set; }
    public string Content { get; set; } = default!;
}
