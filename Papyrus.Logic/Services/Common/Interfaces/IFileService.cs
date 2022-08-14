using KarcagS.Common.Tools.Mongo;

namespace Papyrus.Logic.Services.Common.Interfaces;

public interface IFileService : IMongoCollectionService<Mongo.DataAccess.Entities.File>
{
}
