using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models;

public class PostModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string Content { get; set; } = default!;
}
