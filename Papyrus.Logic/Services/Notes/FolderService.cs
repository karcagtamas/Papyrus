using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Logic.Services.Notes;

public class FolderService : MapperRepository<Folder, string, string>, IFolderService
{
    public FolderService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Folder")
    {
    }

    public void CreateRootFolder(string? userId = null, int? groupId = null)
    {
        ExceptionHelper.Check(ObjectHelper.IsNotNull(userId) || ObjectHelper.IsNotNull(groupId), "Invalid Folder keys", "Server.Message.InvalidFolderKeys");

        Create(new Folder
        {
            UserId = userId,
            GroupId = groupId,
            Title = "ROOT",
            CreatorId = userId,
        });
    }

    public FolderContentDTO GetContent(string? folderId, int? groupId)
    {
        var folder = ObjectHelper.OrElseThrow(TryFind(folderId, groupId), () => new ArgumentException("Folder not found"));

        return new FolderContentDTO
        {
            Folders = Mapper.Map<List<FolderDTO>>(folder.Folders.ToList()),
            Notes = Mapper.Map<List<NoteLightDTO>>(Context.Set<Note>().AsQueryable().Where(x => x.FolderId == folder.Id).Include(x => x.Tags).ThenInclude(x => x.Tag).ToList()),
        };
    }

    private Folder? TryFind(string? folderId, int? groupId)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        return GetList(x => ((folderId == null && x.ParentId == null) || x.Id == folderId) && ((groupId != null && x.GroupId == groupId) || (groupId == null && x.UserId == userId))).FirstOrDefault();
    }
}
