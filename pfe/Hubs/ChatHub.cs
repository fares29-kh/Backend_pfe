using Microsoft.AspNetCore.SignalR;
using pfe.models;

namespace pfe.Hubs
{
    public class ChatHub : Hub
    {
        public async Task sendMessage(Message message) =>
            await Clients.All.SendAsync("receiveMessage", message);

    }
}
