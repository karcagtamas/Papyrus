namespace Papyrus.Shared.Models;

public class TokenModel
{
    public string RefreshToken { get; set; } = default!;
    public string ClientId { get; set; } = default!;
}
