using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models;

public class UserPasswordModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string OldPassword { get; set; } = default!;


    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string NewPassword { get; set; } = default!;
}