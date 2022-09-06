using System.Text.Json.Serialization;

namespace Papyrus.Shared.DTOs;

public class RoleDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string NameEN { get; set; } = default!;

    [JsonIgnore]
    public bool IsAdmin { get => NameEN == "Administrator"; }

    [JsonIgnore]
    public bool IsModerator { get => NameEN == "Moderator"; }
}
