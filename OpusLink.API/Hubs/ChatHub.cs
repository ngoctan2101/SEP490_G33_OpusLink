using Microsoft.AspNetCore.SignalR;
using OpusLink.Entity.Models;
using OpusLink.Entity;

namespace OpusLink.API.Hubs
{
    public class ChatHub : Hub
    {
       

        public async Task SendMessage(int chatBoxId, string user, string messageContent)
        {


            await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", user, messageContent);
        }

      
    }
}
