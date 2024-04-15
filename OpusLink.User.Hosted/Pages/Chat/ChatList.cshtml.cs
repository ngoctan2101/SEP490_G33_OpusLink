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
using OpusLink.Entity.DTO.JobDTO;
using System.Text.Json;
using Microsoft.AspNet.SignalR.Client.Http;
using OpusLink.Shared.Constants;

namespace OpusLink.User.Hosted.Pages.Chat
{
	public class ChatListModel : PageModel
	{
		private readonly HttpClient client = null;
		public int chatBoxId { get; set; }
		public IList<ChatDTO> ChatDTOs { get; set; } = default!;
		
		public IList<MessageDTO> MessageDTOs { get; set; } = default!;
		private readonly IHubContext<ChatHub> _hubContext;
		public int userId { get; set; }
		public string role { get; set; }
		public ChatDTO chatDTO { get; set; }

		public ChatListModel(IHubContext<ChatHub> hubContext)
		{
			_hubContext = hubContext;
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}
		public async Task<IActionResult> OnGetAsync()
		{
			/*            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Chat/GetAllChat");*/
			if (HttpContext.Session.GetInt32("UserId") == null)
			{
				return RedirectToPage("/Account/Login");
			}
			else
			{
				userId = HttpContext.Session.GetInt32("UserId") ?? 0;
				role = HttpContext.Session.GetString("Role");
			}

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+$"/Chat/GetChatBoxByUserId/{userId}/{role}");


			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
			}
			else
			{

			}
			response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Chat/GetMessageById");
			chatDTO = new ChatDTO();

			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				MessageDTOs = JsonConvert.DeserializeObject<List<MessageDTO>>(strData);
			}
			else
			{

			}
			return Page();
		}
		public async Task<IActionResult> OnGetMessageByIdAsync(int chatBoxId )
		{
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }

            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            this.chatBoxId = chatBoxId;
			HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+$"/Chat/GetMessageById/{chatBoxId}");

			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				MessageDTOs = JsonConvert.DeserializeObject<List<MessageDTO>>(strData);

			}
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                role = HttpContext.Session.GetString("Role");
            }
            response = await client.GetAsync(UrlConstant.ApiBaseUrl+$"/Chat/GetChatBoxByUserId/{userId}/{role}");


            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }
            else
            {

            }

			response = await client.GetAsync(UrlConstant.ApiBaseUrl + $"/Chat/GetChatBoxById/{chatBoxId}");


            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                chatDTO = JsonConvert.DeserializeObject<ChatDTO>(strData);
            }
            else
            {

            }
			return Page();

        }

		public async Task<IActionResult> OnGetAddChatBox(int EmployerId, int FreelancerId, int JobId)
		{
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }

            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = false,	
			};
			string json = System.Text.Json.JsonSerializer.Serialize<CreateChatBoxDTO>(new CreateChatBoxDTO() { EmployerID = EmployerId, FreelancerID = FreelancerId, JobID = JobId }, options);
			StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Chat/CreateChatBox", httpContent);

			ChatDTO x = new ChatDTO();
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				x = JsonConvert.DeserializeObject<ChatDTO>(strData);
				
			}

			return RedirectToPage("/Chat/ChatList", new { chatBoxId= x.ChatBoxID, handler = "MessageById" });



		}

        private async Task LoadChatData(int userId, string role)
        {
            var response = await client.GetAsync(UrlConstant.ApiBaseUrl + $"/Chat/GetChatBoxByUserId/{userId}/{role}");

            if (response.IsSuccessStatusCode)
            {
                var strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }
            else
            {
                // Handle error
            }
        }

        private async Task LoadMessagesAsync(int chatBoxId)
        {
            var response = await client.GetAsync(UrlConstant.ApiBaseUrl + $"/Chat/GetMessageById/{chatBoxId}");

            if (response.IsSuccessStatusCode)
            {
                var strData = await response.Content.ReadAsStringAsync();
                MessageDTOs = JsonConvert.DeserializeObject<List<MessageDTO>>(strData);
            }
            else
            {
                // Handle error
            }
        }
    }
}