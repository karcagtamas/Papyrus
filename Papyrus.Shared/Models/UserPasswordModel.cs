using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models;

public class UserPasswordModel
{
    [Required(ErrorMessage = "Field is required")]
    public string OldPassword { get; set; } = default!;


    [Required(ErrorMessage = "Field is required")]
    public string NewPassword { get; set; } = default!;
}