using AutoMapper;
using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;

namespace Papyrus.Logic.Services.Notes;

public class FolderService : MapperRepository<Folder, string, string>, IFolderService
{
    public FolderService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Folder")
    {
    }

    public void createRootFolder(string? userId = null, int? groupId = null)
    {
        ExceptionHelper.Check(ObjectHelper.IsNotNull(userId) || ObjectHelper.IsNotNull(groupId), "Invalid Folder keys", "Server.Message.InvalidFolderKeys");
        // TODO: Add resource key translations to error message

        Create(new Folder
        {
            UserId = userId,
            GroupId = groupId,
            Title = "ROOT",
            CreatorId = userId,
        });
    }
}
