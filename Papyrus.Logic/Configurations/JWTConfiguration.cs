namespace Papyrus.Logic.Configurations;

public class JWTConfiguration
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int ExpirationInMinutes { get; set; } = 60;
}
