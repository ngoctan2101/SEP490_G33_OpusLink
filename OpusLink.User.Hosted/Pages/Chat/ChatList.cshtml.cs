using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using OpusLink.API.Hubs;
using Microsoft.JSInterop;

namespace OpusLink.User.Hosted.Pages.Chat
{
    public class ChatListModel : PageModel
    {
        private readonly HttpClient client = null;
        public int chatBoxId { get; set; }
        public IList<ChatDTO> ChatDTOs { get; set; } = default!;
        public IList<MessageDTO> MessageDTOs { get; set; } = default!;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatListModel(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Chat/GetAllChat");

           
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }
            else
            {

            }
             response = await client.GetAsync("https://localhost:7265/api/Chat/GetChatBoxById");


            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }
            else
            {

            }
            response = await client.GetAsync("https://localhost:7265/api/Chat/GetMessageById");


            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                MessageDTOs = JsonConvert.DeserializeObject<List<MessageDTO>>(strData);
            }
            else
            {

            }
            
        }
        public async Task OnGetMessageByIdAsync(int chatBoxId)
        {
            this.chatBoxId = chatBoxId;
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7265/api/Chat/GetMessageById/{chatBoxId}");

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                MessageDTOs = JsonConvert.DeserializeObject<List<MessageDTO>>(strData);
                
            }
             response = await client.GetAsync("https://localhost:7265/api/Chat/GetAllChat");

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }

           

        }
        public async Task SendMessage(int chatBoxId, string user, string messageContent)
        {
            await _hubContext.Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", user, messageContent);
        }


    }
}