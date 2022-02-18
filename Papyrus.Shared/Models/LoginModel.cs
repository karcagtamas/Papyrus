using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models;

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;
}
