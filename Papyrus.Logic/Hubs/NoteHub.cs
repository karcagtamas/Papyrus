using KarcagS.Shared.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Papyrus.Logic.Hubs;

public class NoteHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var note = ExtractNoteKey(true);

        await Groups.AddToGroupAsync(Context.ConnectionId, note);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var note = ExtractNoteKey();

        if (!string.IsNullOrEmpty(note))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, note);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string ExtractNoteKey(bool required = false)
    {
        var editor = Context.GetHttpContext()?.Request.Query["Note"];

        if (required && ObjectHelper.IsNull(editor))
        {
            throw new ArgumentNullException("Note key is required field in request query");
        }

        return editor.ToString() ?? string.Empty;
    }
}
