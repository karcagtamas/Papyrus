using AutoMapper;
using KarcagS.Common.Tools.Mongo;
using Papyrus.Logic.Services.Common.Interfaces;
using Papyrus.Mongo.DataAccess.Configurations;

namespace Papyrus.Logic.Services.Common;

public class FileService : MongoCollectionService<Mongo.DataAccess.Entities.File, CollectionConfiguration>, IFileService
{
    public FileService(IMongoService<CollectionConfiguration> mongoService, IMapper mapper) : base(mongoService, mapper, (conf) => conf.Files)
    {
    }
}
