using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Groups;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes;

public class TagService : MapperRepository<Tag, int, string>, ITagService
{
    public TagService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Tag")
    {
    }

    public List<TagDTO> GetByGroup(int groupId) => GetMappedList<TagDTO>(x => x.GroupId == groupId).ToList();

    public TagPathDTO GetPath(int id)
    {
        return new TagPathDTO
        {
            Id = id,
            Path = GetPathString(id)
        };
    }

    public List<GroupTagTreeItemDTO> GetTreeByGroup(int groupId)
    {
        var tags = GetList(x => x.GroupId == groupId && x.ParentId == null);

        return tags.Select(x => Map(x)).ToList();
    }

    private GroupTagTreeItemDTO Map(Tag tag)
    {
        return new GroupTagTreeItemDTO
        {
            Id = tag.Id,
            Caption = tag.Caption,
            Description = tag.Description,
            Color = tag.Color,
            Children = tag.Children.Select(x => Map(x)).ToList(),
        };
    }

    public string GetPathString(int id)
    {
        var tag = Get(id);

        if (tag.ParentId is null)
        {
            return tag.Caption;
        }

        return $"{GetPathString((int)tag.ParentId)}/{tag.Caption}";
    }
}
