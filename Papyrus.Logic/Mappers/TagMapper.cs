using AutoMapper;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Mappers;

public class TagMapper : Profile
{
	public TagMapper()
	{
		CreateMap<Tag, TagDTO>();
		CreateMap<TagModel, Tag>();
    }
}
