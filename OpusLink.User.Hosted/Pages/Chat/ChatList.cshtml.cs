using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Entity.Models.JOB;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace OpusLink.User.Hosted.Pages.Chat
{
    public class ChatListModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<ChatDTO> ChatDTOs { get; set; } = default!;
        public IList<MessageDTO> MessageDTOs { get; set; } = default!;
        
        public ChatListModel()
        {
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



    }
}