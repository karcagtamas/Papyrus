using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models.Admin;

public class UserSettingModel
{
    [LocalizedRequired]
    public string RoleId { get; set; } = default!;

    [LocalizedRequired]
    public bool Disabled { get; set; }
}
