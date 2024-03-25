﻿using Microsoft.AspNetCore.Mvc;
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


			HttpResponseMessage response = await client.GetAsync($"https://localhost:7265/api/Chat/GetChatBoxByUserId/{userId}/{role}");


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
			return Page();
		}
		public async Task<IActionResult> OnGetMessageByIdAsync(int chatBoxId)
		{
			this.chatBoxId = chatBoxId;
			HttpResponseMessage response = await client.GetAsync($"https://localhost:7265/api/Chat/GetMessageById/{chatBoxId}");

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
            response = await client.GetAsync($"https://localhost:7265/api/Chat/GetChatBoxByUserId/{userId}/{role}");


            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                ChatDTOs = JsonConvert.DeserializeObject<List<ChatDTO>>(strData);
            }
            else
            {

            }
			return Page();

        }
		public async Task<IActionResult> OnGetAddChatBox(int EmployerId, int FreelancerId, int JobId)
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = false,	
			};
			string json = System.Text.Json.JsonSerializer.Serialize<CreateChatBoxDTO>(new CreateChatBoxDTO() { EmployerID = EmployerId, FreelancerID = FreelancerId, JobID = JobId }, options);
			StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Chat/CreateChatBox", httpContent);

			ChatDTO x = new ChatDTO();
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				x = JsonConvert.DeserializeObject<ChatDTO>(strData);
				
			}

			return RedirectToPage("/Chat/ChatList", new { chatBoxId= x.ChatBoxID, handler = "MessageById" });



		}
		public async Task SendMessage(int chatBoxId, string user, string messageContent)
		{
			await _hubContext.Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", user, messageContent);
		}


	}
}