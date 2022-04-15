using Microsoft.AspNetCore.SignalR;

namespace Papyrus.Logic.Hubs;
public class EditorHub : Hub
{
    public async Task SendTest() 
    {
        await Clients.All.SendAsync("ReceiveTest", DateTime.Now);
    }
}
