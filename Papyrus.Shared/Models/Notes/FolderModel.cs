using System.ComponentModel.DataAnnotations;
using Papyrus.Shared.Attributes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Shared.Models.Notes;

public class FolderModel
{

    [LocalizedRequired(ErrorMessage = "Field is required")]
    [LocalizedMaxLength(40, ErrorMessage = "Maximum length is 40")]
    public string Name { get; set; } = default!;

    [Required]
    public string ParentId { get; set; } = default!;
    public int? GroupId { get; set; }

    public FolderModel()
    {

    }

    public FolderModel(FolderDTO folder)
    {
        Name = folder.Name;
    }
}
