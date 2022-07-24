using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Shared.Models.Notes;

public class NoteModel
{
    public string Title { get; set; } = default!;

    public NoteModel()
    {

    }

    public NoteModel(NoteLightDTO dto)
    {
        Title = dto.Title;
    }
}
