using Microsoft.AspNetCore.SignalR;
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

    public async Task Change(string content)
    {
        await Clients.All.SendAsync(EditorHubEvents.EditorChanged, content);
    }

    public async Task Disconnect()
    {
    }
}
