using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Groups;

public class GroupModel
{
    [Required]
    public string Name { get; set; } = default!;
}
