namespace Papyrus.Shared.DTOs.Profile;

public class ApplicationDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string PublicId { get; set; } = default!;
    public string SecretId { get; set; } = default!;
    public DateTime Creation { get; set; }
    public DateTime LastUpdate { get; set; }
}
