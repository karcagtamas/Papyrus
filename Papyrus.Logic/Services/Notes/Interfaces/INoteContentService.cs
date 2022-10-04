using KarcagS.Common.Tools.Mongo;
using Papyrus.Mongo.DataAccess.Entities;

namespace Papyrus.Logic.Services.Notes.Interfaces;

public interface INoteContentService : IMongoCollectionService<NoteContent>
{
    void UpdateContent(string id, string content);
    List<NoteContent> Search(List<string> srcIds, string text);
}
