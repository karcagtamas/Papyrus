using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "E-mail is required")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = default!;

    public string FullName { get; set; } = default!;
}