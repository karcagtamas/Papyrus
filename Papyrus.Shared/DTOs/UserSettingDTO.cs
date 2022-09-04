namespace Papyrus.Shared.DTOs;

public class UserSettingDTO
{
    public string Id { get; set; } = default!;
    public string RoleId { get; set; } = default!;
    public bool Disabled { get; set; }
}
