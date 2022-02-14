using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models;

public class UserPasswordModel
{
    [Required(ErrorMessage = "Field is required")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Field is required")]
    public string NewPassword { get; set; }
}