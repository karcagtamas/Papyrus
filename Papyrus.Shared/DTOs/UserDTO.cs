namespace Papyrus.Shared.DTOs;

public class UserDTO : UserLightDTO
{
    public DateTime LastLogin { get; set; }
    public DateTime Registration { get; set; }
    public DateTime? BirthDay { get; set; }
    public bool Disabled { get; set; }
    public string? ImageTitle { get; set; } = default!;
    public byte[]? ImageData { get; set; } = default!;
}
