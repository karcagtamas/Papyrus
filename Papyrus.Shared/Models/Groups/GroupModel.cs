using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models.Groups;

public class GroupModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string Name { get; set; } = default!;
}
