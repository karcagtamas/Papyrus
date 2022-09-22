using KarcagS.Common.Tools.Services;
using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Papyrus.Logic.Authorization;
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
    private readonly INoteContentService noteContentService;
    private readonly IAuthorizationService authorization;
    private readonly IUtilsService<string> utils;

    public EditorHub(INoteService noteService, IEditorService editorService, IUserService userService, INoteContentService noteContentService, IAuthorizationService authorization, IUtilsService<string> utils)
    {
        this.noteService = noteService;
        this.editorService = editorService;
        this.userService = userService;
        this.noteContentService = noteContentService;
        this.authorization = authorization;
        this.utils = utils;
    }

    public override async Task OnConnectedAsync()
    {
        var editor = ExtractEditorKey(true);

        await Groups.AddToGroupAsync(Context.ConnectionId, editor);
        editorService.AddMember(Context.UserIdentifier!, editor, Context.ConnectionId);
        var user = userService.GetMapped<UserLightDTO>(Context.UserIdentifier!);

        await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberJoined, user);

        await base.OnConnectedAsync();
    }

    [Authorize]
    public async Task Share(string editor, byte[] value)
    {
        var note = noteService.GetOptional(editor);

        if (ObjectHelper.IsNull(note))
        {
            return;
        }

        if (!(await authorization.AuthorizeAsync(utils.GetRequiredUserPrincipal(), note.Id, NotePolicies.EditNote.Requirements)).Succeeded)
        {
            return;
        }

        var originalContent = noteContentService.Get(note.ContentId);

        if (ObjectHelper.IsNull(originalContent))
        {
            return;
        }

        string content = Encoding.UTF8.GetString(value);
        List<Diff> diffs = new DiffMatchPatch().DiffMain(originalContent.Content, content);
        if (diffs.Count > 0)
        {
            Diff.ApplyDiffs(originalContent.Content, diffs, async (res) =>
            {
                noteContentService.UpdateContent(originalContent.Id, res);

                note.ContentLastEdit = DateTime.Now;
                noteService.Update(note);

                await Clients.OthersInGroup(editor).SendAsync(EditorHubEvents.EditorChanged, Encoding.UTF8.GetBytes(res));
            });
        }
    }

    [Authorize]
    public async Task UpdateNote(string editor, NoteChangeEventArgs args) => await Clients.OthersInGroup(editor).SendAsync(EditorHubEvents.EditorNoteUpdated, args);

    [Authorize]
    public async Task DeleteNote(string editor) => await Clients.OthersInGroup(editor).SendAsync(EditorHubEvents.EditorNoteDeleted);

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var editor = ExtractEditorKey();

        if (!string.IsNullOrEmpty(editor))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, editor);
            editorService.RemoveMember(Context.UserIdentifier!, editor, Context.ConnectionId);

            await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberLeft, Context.UserIdentifier);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string ExtractEditorKey(bool required = false)
    {
        var editor = Context.GetHttpContext()?.Request.Query["Editor"];

        if (required && ObjectHelper.IsNull(editor))
        {
            throw new ArgumentNullException("Editor key is required field in request query");
        }

        return editor ?? string.Empty;
    }
}
