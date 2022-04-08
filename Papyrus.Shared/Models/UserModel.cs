using System.ComponentModel.DataAnnotations;
using Papyrus.Shared.DTOs;

namespace Papyrus.Shared.Models;

public class UserModel
{
    [Required(ErrorMessage = "Field is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Field is required")]
    public string Email { get; set; }

    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string? FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    public UserModel()
    {
        UserName = default!;
        Email = default!;
        FullName = default!;
    }

    public UserModel(UserDTO dto)
    {
        UserName = dto.UserName;
        Email = dto.Email;
        FullName = dto.FullName;
        BirthDay = dto.BirthDay;
    }
}
