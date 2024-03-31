using Microsoft.AspNetCore.SignalR;
using OpusLink.Entity.Models;
using OpusLink.Entity;

namespace OpusLink.API.Hubs
{
    public class ChatHub : Hub
    {
       

        public async Task SendMessage(int chatBoxId, bool fromEmployer, string messageContent)
        {


            await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", fromEmployer, messageContent);
        }
        public async Task AddToGroup(int chatBoxId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        }

        public async Task RemoveFromGroup(int chatBoxId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        }

    }
}
