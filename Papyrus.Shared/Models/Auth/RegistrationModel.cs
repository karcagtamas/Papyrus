using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models.Auth;

public class RegistrationModel
{
    [LocalizedRequired(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = default!;

    [LocalizedRequired(ErrorMessage = "E-mail is required")]
    [LocalizedEmailAddress]
    public string Email { get; set; } = default!;

    [LocalizedRequired(ErrorMessage = "Password is required")]
    [LocalizedMinLength(8, ErrorMessage = "Minimum length is 8")]
    [LocalizedContainsAlpha(ErrorMessage = "Need to contain at least one letter")]
    [LocalizedContainsCapitalAlpha(ErrorMessage = "Need to contain at least one capital letter")]
    [LocalizedContainsNumeric(ErrorMessage = "Need to contain at least one numeric character")]
    [LocalizedContainsSpecial(ErrorMessage = "Need to contain at least one special character")]
    public string Password { get; set; } = default!;

    [LocalizedMaxLength(100)]
    public string FullName { get; set; } = default!;
}
