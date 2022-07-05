using Papyrus.Shared.DTOs.Notes;
using System.ComponentModel.DataAnnotations;

namespace Papyrus.Shared.Models.Notes;

public class TagModel
{
    public int? GroupId { get; set; }

    public int? ParentId { get; set; }

    [Required(ErrorMessage = "Field is required")]
    public string Caption { get; set; } = default!;

    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Field is required")]
    public string Color { get; set; } = "#FFFFFF00";

    public TagModel()
    {

    }

    public TagModel(TagDTO dto, int? groupId = null)
    {
        Caption = dto.Caption;
        Description = dto.Description;
        Color = dto.Color;
        GroupId = groupId;
    }
}
