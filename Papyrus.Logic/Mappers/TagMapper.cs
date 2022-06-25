using AutoMapper;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Mappers;

public class TagMapper : Profile
{
	public TagMapper()
	{
		CreateMap<Tag, GroupTagDTO>();
	}
}
