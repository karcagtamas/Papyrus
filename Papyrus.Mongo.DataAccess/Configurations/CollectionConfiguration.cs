using KarcagS.Common.Tools;

namespace Papyrus.Mongo.DataAccess.Configurations;

public class CollectionConfiguration : MongoCollectionConfiguration
{
    public string Files { get; set; } = default!;
    public string NoteContents { get; set; } = default!;
}
