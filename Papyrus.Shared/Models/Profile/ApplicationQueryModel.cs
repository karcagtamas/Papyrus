using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Profile;

public class ApplicationQueryModel
{
    [Required]
    public string PublicId { get; set; } = default!;

    [Required]
    public string SecretId { get; set; } = default!;
}
