using KarcagS.Common.Tools.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Papyrus.Mongo.DataAccess.Entities;

public class NoteContent : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
    public string Content { get; set; } = default!;
}
