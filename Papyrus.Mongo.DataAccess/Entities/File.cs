using KarcagS.Common.Tools.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Papyrus.Mongo.DataAccess.Enums;

namespace Papyrus.Mongo.DataAccess.Entities;

public class File : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public long Size { get; set; }
    public byte[] Content { get; set; } = default!;
    public FileCategory Category { get; set; }
}
