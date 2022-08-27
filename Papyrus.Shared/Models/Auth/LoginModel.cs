using Papyrus.Shared.Attributes;

namespace Papyrus.Shared.Models.Auth;

public class LoginModel
{
    [LocalizedRequired(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = default!;

    [LocalizedRequired(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;
}
