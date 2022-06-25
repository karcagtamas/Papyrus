using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupModel
{
    [Required(ErrorMessage = "Field is required")]
    public string Name { get; set; } = default!;
}
