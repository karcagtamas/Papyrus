using System.Text.RegularExpressions;
using AutoMapper;
using KarcagS.Common.Tools.Mongo;
using MongoDB.Driver;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Mongo.DataAccess.Configurations;
using Papyrus.Mongo.DataAccess.Entities;

namespace Papyrus.Logic.Services.Notes;

public class NoteContentService : MongoCollectionService<NoteContent, CollectionConfiguration>, INoteContentService
{
    public NoteContentService(IMongoService<CollectionConfiguration> mongoService, IMapper mapper) : base(mongoService, mapper, (conf) => conf.NoteContents)
    {
    }

    public List<NoteContent> Search(List<string> srcIds, string text)
    {
        return Collection.Find(x => srcIds.Contains(x.Id))
            .ToList()
            .Where(x =>
            {
                return Regex.Replace(x.Content, "<.*?>", "").ToLower().Contains(text.ToLower());
            })
            .ToList();
    }

    public void UpdateContent(string id, string content) => Collection.UpdateOne(Builders<NoteContent>.Filter.Eq(x => x.Id, id), Builders<NoteContent>.Update.Set(x => x.Content, content));
}
