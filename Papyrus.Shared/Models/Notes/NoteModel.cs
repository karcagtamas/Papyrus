using Papyrus.Shared.Attributes;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Shared.Models.Notes;

public class NoteModel
{
    [LocalizedRequired(ErrorMessage = "Field is required")]
    public string Title { get; set; } = default!;
    public List<int> Tags { get; set; } = new();
    public bool Public { get; set; }
    public bool Archived { get; set; }

    public NoteModel()
    {

    }

    public NoteModel(NoteLightDTO dto)
    {
        Title = dto.Title;
        Public = dto.Public;
        Archived = dto.Archived;
    }
}
