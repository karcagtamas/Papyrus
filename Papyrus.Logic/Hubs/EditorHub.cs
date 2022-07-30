using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Papyrus.Shared.DiffMatchPatch;
using Papyrus.Shared.HubEvents;

namespace Papyrus.Logic.Hubs;

public class EditorHub : Hub
{
    public EditorHub()
    {
    }

    [Authorize]
    public async Task Connect(string editor)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, editor);
        await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberChanged, Context.UserIdentifier, EditorMemberChange.Join);
    }

    [Authorize]
    public async Task Share(string editor, List<TransportDiff> diffs)
    {
        if (diffs.Count > 0)
        {
            await Clients.Group(editor).SendAsync(EditorHubEvents.EditorChanged, diffs);
        }
    }

    [Authorize]
    public async Task UpdateNote(string editor, NoteChangeEventArgs args) => await Clients.Group(editor).SendAsync(EditorHubEvents.EditorNoteUpdated, args);

    [Authorize]
    public async Task Disconnect(string editor)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, editor);
        await Clients.Group(editor).SendAsync(EditorHubEvents.EditorMemberChanged, Context.UserIdentifier, EditorMemberChange.Leave);
    }
}
