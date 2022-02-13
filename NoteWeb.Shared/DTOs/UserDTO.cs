namespace Papyrus.Shared.DTOs;

public class UserDTO
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public DateTime LastLogin { get; set; }
    public DateTime Registration { get; set; }
    public DateTime? BirthDay { get; set; }
    public bool Disabled { get; set; }
    public string ImageTitle { get; set; } = "";
    public byte[] ImageData { get; set; } = null!;
    public string Country { get; set; } = "";
    public string Bio { get; set; } = "";
}
