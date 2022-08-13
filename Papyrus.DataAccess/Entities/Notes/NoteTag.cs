using System.ComponentModel.DataAnnotations;

namespace Papyrus.DataAccess.Entities.Notes;

public class NoteTag
{
    [Required]
    public string NoteId { get; set; } = default!;

    [Required]
    public int TagId { get; set; }

    public virtual Note Note { get; set; } = default!;

    public virtual Tag Tag { get; set; } = default!;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is NoteTag tag)
        {
            return tag.TagId == TagId && tag.NoteId == NoteId;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
