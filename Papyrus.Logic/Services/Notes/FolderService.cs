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
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Services.Notes;

public class FolderService : MapperRepository<Folder, string, string>, IFolderService
{
    public FolderService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Folder")
    {
    }

    public void CreateFolder(FolderModel model)
    {
        var parentFolder = Get(model.ParentId);

        ExceptionHelper.Check(parentFolder.GroupId == model.GroupId, "Invalid Folder keys", "Server.Message.InvalidFolderKeys");
        ExceptionHelper.Check(parentFolder.Folders.ToList().All(f => f.Name != model.Name), () => new ArgumentException("The name of the Folder has to be unique"));

        var folder = Mapper.Map<Folder>(model);

        folder.UserId = parentFolder.UserId;
        folder.GroupId = parentFolder.GroupId;

        Create(folder);
    }

    public void CreateRootFolder(string? userId = null, int? groupId = null)
    {
        ExceptionHelper.Check(ObjectHelper.IsNotNull(userId) || ObjectHelper.IsNotNull(groupId), "Invalid Folder keys", "Server.Message.InvalidFolderKeys");

        Create(new Folder
        {
            UserId = userId,
            GroupId = groupId,
            Name = "ROOT",
            CreatorId = userId,
        });
    }

    public void EditFolder(string id, FolderModel model)
    {
        var folder = Get(id);

        ExceptionHelper.Check(folder.GroupId == model.GroupId, "Invalid Folder keys", "Server.Message.InvalidFolderKeys");
        ExceptionHelper.Check(ObjectHelper.IsNull(folder.Parent) || folder.Parent.Folders.ToList().All(f => !NamesAreEqual(f.Name, model.Name)), () => new ArgumentException("The name of the Folder has to be unique"));

        UpdateByModel(id, model);
    }

    public bool Exists(string parentFolderId, string name)
    {
        var folder = Get(parentFolderId);

        return folder.Folders.ToList().Any(x => NamesAreEqual(x.Name, name));
    }

    public FolderContentDTO GetContent(string? folderId, int? groupId)
    {
        var folder = ObjectHelper.OrElseThrow(TryFind(folderId, groupId), () => new ArgumentException("Folder not found"));

        return new FolderContentDTO
        {
            ParentFolder = Mapper.Map<FolderDTO>(folder),
            Folders = Mapper.Map<List<FolderDTO>>(
                folder.Folders
                    .OrderBy(x => x.Name)
                    .ToList()),
            Notes = Mapper.Map<List<NoteLightDTO>>(
                Context.Set<Note>().AsQueryable()
                    .Where(x => x.FolderId == folder.Id)
                    .Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .OrderBy(x => x.Title)
                    .ToList()),
            Path = GetFolderPath(folder),
        };
    }

    private Folder? TryFind(string? folderId, int? groupId)
    {
        var userId = Utils.GetRequiredCurrentUserId();
        return ObjectHelper.IsNull(folderId)
            ? GetList(x => x.ParentId == null && ((groupId != null && x.GroupId == groupId) || x.UserId == userId)).FirstOrDefault()
            : GetList(x => x.Id == folderId && ((groupId != null && x.GroupId == groupId) || x.UserId == userId)).FirstOrDefault();
    }

    private bool NamesAreEqual(string n1, string n2) => n1.ToLower() == n2.ToLower();

    private List<FolderPathDTO> GetFolderPath(Folder child)
    {
        List<Folder> folders = new();
        var current = new FolderPathDTO { FolderId = child.Id, Name = child.Name };

        if (ObjectHelper.IsNotNull(child.ParentId))
        {
            var list = GetFolderPath(child.Parent!);
            list.Add(current);
            return list;
        }

        current.IsRoot = true;
        return new List<FolderPathDTO> { current };
    }
}
