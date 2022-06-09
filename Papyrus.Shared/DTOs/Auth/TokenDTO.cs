namespace Papyrus.Shared.DTOs.Auth;

public class TokenDTO
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
