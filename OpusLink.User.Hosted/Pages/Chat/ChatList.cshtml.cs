using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.Models;
using OpusLink.Entity.Models.JOB;
using System.Net.Http.Headers;

namespace OpusLink.User.Hosted.Pages.Chat
{
    public class ChatListModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<ChatBox> ChatBoxes { get; set; } = default;
        public ChatListModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job3API/GetAllJob");

            response = await client.GetAsync("https://localhost:7265/api/Job3API/GetAllChildCategory?parentId=" + 0);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatBoxes = JsonConvert.DeserializeObject<List<GetJobResponse>>(strData);
            }
            else
            {

            }
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatBoxes = JsonConvert.DeserializeObject<List<ChatBox>>(strData);
            }
            else
            {

            }
        }
    }
}