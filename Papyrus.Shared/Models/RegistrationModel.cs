using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "E-mail is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public string FullName { get; set; }
}