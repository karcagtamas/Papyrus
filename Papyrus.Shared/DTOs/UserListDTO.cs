namespace Papyrus.Shared.DTOs;

public class UserListDTO
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateTime LastLogin { get; set; }
    public DateTime Registration { get; set; }
    public bool Disabled { get; set; }
}
