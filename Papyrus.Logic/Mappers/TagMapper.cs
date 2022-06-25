using AutoMapper;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Groups;

namespace Papyrus.Logic.Mappers;

public class TagMapper : Profile
{
	public TagMapper()
	{
		CreateMap<Tag, TagDTO>();
		CreateMap<GroupTagModel, Tag>();
    }
}
