using System.ComponentModel.DataAnnotations;
using Papyrus.Shared.DTOs;

namespace Papyrus.Shared.Models;

public class UserModel
{
    [Required(ErrorMessage = "Field is required")] public string UserName { get; set; }

    [Required(ErrorMessage = "Field is required")] public string Email { get; set; }

    [MaxLength(100, ErrorMessage = "Max length is 100")] public string FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    [MaxLength(60, ErrorMessage = "Max length is 60")] public string Country { get; set; }

    [MaxLength(2000, ErrorMessage = "Max length is 2000")] public string Bio { get; set; }

    public UserModel()
    {
    }

    public UserModel(UserDTO dto)
    {
        UserName = dto.UserName;
        Email = dto.Email;
        FullName = dto.FullName;
        BirthDay = dto.BirthDay;
        Country = dto.Country;
        Bio = dto.Bio;
    }
}
