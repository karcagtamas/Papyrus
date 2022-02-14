namespace Papyrus.Shared.DTOs;

public class UserListDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime Registration { get; set; }
    public bool Disabled { get; set; }
}
