using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models;

public class UserPasswordModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string OldPassword { get; set; } = default!;


    [LocalizedRequired(ErrorMessage = "Field is required")]
    [LocalizedMinLength(8, ErrorMessage = "Minimum length is 8")]
    [LocalizedContainsAlpha(ErrorMessage = "Need to contain at least one letter")]
    [LocalizedContainsCapitalAlpha(ErrorMessage = "Need to contain at least one capital letter")]
    [LocalizedContainsNumeric(ErrorMessage = "Need to contain at least one numeric character")]
    [LocalizedContainsSpecial(ErrorMessage = "Need to contain at least one special character")]
    public string NewPassword { get; set; } = default!;
}
