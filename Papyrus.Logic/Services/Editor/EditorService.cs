using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities.Editor;
using Papyrus.Logic.Services.Editor.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services.Editor;

public class EditorService : IEditorService
{
    private readonly Persistence<string> persistence;
    private readonly IMapper mapper;

    public EditorService(PapyrusContext context, IUtilsService<string> utilsService, IMapper mapper)
    {
        persistence = new Persistence<string>(context, utilsService);
        this.mapper = mapper;
    }

    public void AddMember(string userId, string noteId, string connectionId)
    {
        persistence.DeleteRange<string, EditorMember>(persistence.GetList<string, EditorMember>(x => (x.UserId == userId && x.NoteId == noteId) || x.Date.AddDays(1) < DateTime.Now));
        persistence.Create<string, EditorMember>(new EditorMember
        {
            UserId = userId,
            NoteId = noteId,
            ConnectionId = connectionId,
            Date = DateTime.Now
        });
    }

    public List<UserLightDTO> GetMembers(string noteId) => mapper.Map<List<UserLightDTO>>(persistence.GetList<string, EditorMember>(x => x.NoteId == noteId).Select(x => x.User).ToList());

    public void RemoveMember(string userId, string noteId, string connectionId)
    {
        var member = persistence.GetList<string, EditorMember>(x => x.UserId == userId && x.NoteId == noteId && x.ConnectionId == connectionId).FirstOrDefault();

        ObjectHelper.WhenNotNull(member, m => persistence.Delete<string, EditorMember>(m));
    }
}
