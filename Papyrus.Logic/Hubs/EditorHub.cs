using Microsoft.AspNetCore.SignalR;
using Papyrus.Shared.DiffMatchPatch;
using Papyrus.Shared.HubEvents;

namespace Papyrus.Logic.Hubs;

public class EditorHub : Hub
{
    public async Task SendTest()
    {
        await Clients.All.SendAsync("ReceiveTest", DateTime.Now);
    }

    public async Task Connect()
    {

    }

    public async Task Change(List<TransportDiff> diffs)
    {
        if (diffs.Count > 0)
        {
            await Clients.All.SendAsync(EditorHubEvents.EditorChanged, diffs);
        }
    }

    public async Task Disconnect()
    {
    }
}
