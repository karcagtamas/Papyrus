namespace NoteWeb.Logic.Configurations;

public class JWTConfiguration
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public int ExpirationInMinutes { get; set; } = 60;
}
