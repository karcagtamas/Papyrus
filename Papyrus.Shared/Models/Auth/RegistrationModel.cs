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
    public string Password { get; set; } = default!;

    [LocalizedMaxLength(100)]
    public string FullName { get; set; } = default!;
}