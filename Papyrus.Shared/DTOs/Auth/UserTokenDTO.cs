namespace Papyrus.Shared.DTOs.Auth;

public class UserTokenDTO
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
