using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models.Profile;

public class ApplicationModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    [LocalizedMaxLength(40, ErrorMessage = "Max length is 40")]
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
