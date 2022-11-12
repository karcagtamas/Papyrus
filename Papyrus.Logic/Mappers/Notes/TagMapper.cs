using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Mappers.Notes;

public class TagMapper : AutoMapper.Profile
{
    public TagMapper()
    {
        CreateMap<Tag, TagDTO>();
        CreateMap<Tag, NoteTagDTO>();
        CreateMap<TagModel, Tag>();
    }
}
