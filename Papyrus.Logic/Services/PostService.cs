using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;

namespace Papyrus.Logic.Services;

public class PostService : MapperRepository<Post, int, string>, IPostService
{
    public PostService(PapyrusContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Post")
    {
    }
}
