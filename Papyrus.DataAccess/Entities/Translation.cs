using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities;

public class Translation
{
    [Required]
    public string Key { get; set; } = default!;

    [Required]
    public string Segment { get; set; } = default!;

    [Required]
    public string Language { get; set; } = default!;

    [Required]
    public string Value { get; set; } = default!;
}
