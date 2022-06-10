using AutoMapper;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Shared.DTOs.Groups;

namespace Papyrus.Logic.Services.Groups;

public class GroupService : MapperRepository<Group, int, string>, IGroupService
{
    public GroupService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Group")
    {
    }

    public List<GroupListDTO> GetUserList()
    {
        var user = Utils.GetRequiredCurrentUserId();

        if (user is null)
        {
            throw new ServerException("User not found");
        }

        return GetMappedList<GroupListDTO>(x => x.OwnerId == user)
            .ToList();
    }
}
