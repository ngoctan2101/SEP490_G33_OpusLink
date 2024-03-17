using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.Chat
{
    public interface IChatHubService
    {
        Task SendMessageToHub(int chatBoxId, string user, string messageContent);
    }

    
}
