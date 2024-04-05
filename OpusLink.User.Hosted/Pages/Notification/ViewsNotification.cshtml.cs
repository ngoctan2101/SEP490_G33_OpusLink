using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.NotificationDTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.Notification
{
	public class ViewsNotificationModel : PageModel
	{
		[BindProperty]

		public List<NotificationDTO> noti { get; set; } = null!;
		private readonly HttpClient client = null;
		private string ServiceMangaUrl = "";
		public ViewsNotificationModel()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ServiceMangaUrl = UrlConstant.ApiBaseUrl;
			//_validationService = validateService;
		}
		public async Task OnGetAsync(int UserId)
		{

			HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"/Notification/GetAllNotification/{UserId}");
			if (responseUser.IsSuccessStatusCode)
			{
				string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
				var optionUser = new JsonSerializerOptions()
				{ PropertyNameCaseInsensitive = true };
				noti = JsonSerializer.Deserialize<List<NotificationDTO>>(responseBodyUser, optionUser);
			}


		}

	}
}
