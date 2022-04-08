namespace Papyrus.Shared.DTOs;

public class UserDTO
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? FullName { get; set; } = default!;
    public DateTime LastLogin { get; set; }
    public DateTime Registration { get; set; }
    public DateTime? BirthDay { get; set; }
    public bool Disabled { get; set; }
    public string? ImageTitle { get; set; } = default!;
    public byte[]? ImageData { get; set; } = default!;
}
