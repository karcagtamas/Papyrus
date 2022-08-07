using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Papyrus.Logic.Services.Editor.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Shared.DiffMatchPatch;
using Papyrus.Shared.DTOs;
using Papyrus.Shared.HubEvents;
using System.Text;

namespace Papyrus.Logic.Hubs;

public class EditorHub : Hub
{
    private readonly INoteService noteService;
    private readonly IEditorService editorService;
    private readonly IUserService userService;

    public EditorHub(INoteService noteService, IEditorService editorService, IUserService userService)
    {
        this.noteService = noteService;
        this.editorService = editorService;
        this.userService = userService;
    }

    [Authorize]
    public async Task Connect(string editor)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, editor);
        editorService.AddMember(Context.UserIdentifier!, editor, Context.ConnectionId);
        var user = userService.GetMapped<UserLightDTO>(Context.UserIdentifier!);

        await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberJoined, user);
    }

    [Authorize]
    public void Share(string editor, byte[] value)
    {
        var note = noteService.GetOptional(editor);

        if (ObjectHelper.IsNull(note))
        {
            return;
        }

        string originalContent = note.Content;
        string content = Encoding.UTF8.GetString(value);
        List<Diff> diffs = new DiffMatchPatch().DiffMain(originalContent, content);
        if (diffs.Count > 0)
        {
            Diff.ApplyDiffs(originalContent, diffs, async (res) =>
            {
                note.Content = res;
                noteService.Update(note);

                await Clients.Group(editor).SendAsync(EditorHubEvents.EditorChanged, Encoding.UTF8.GetBytes(res));
            });
        }
    }

    [Authorize]
    public async Task UpdateNote(string editor, NoteChangeEventArgs args) => await Clients.Group(editor).SendAsync(EditorHubEvents.EditorNoteUpdated, args);

    [Authorize]
    public async Task Disconnect(string editor)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, editor);
        editorService.RemoveMember(Context.UserIdentifier!, editor, Context.ConnectionId);

        await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberLeft, Context.UserIdentifier);
    }
}
