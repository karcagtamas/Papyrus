using Papyrus.Shared.Attributes;
using Papyrus.Shared.DTOs;

namespace Papyrus.Shared.Models;

public class UserModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string UserName { get; set; }

    [LocalizedRequired(ErrorMessage = "Field is required")]
    [LocalizedEmailAddress]
    public string Email { get; set; }

    [LocalizedMaxLength(100, ErrorMessage = "Max length is 100")]
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
