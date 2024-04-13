using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.FeedbackDTO;
using OpusLink.Entity.DTO.FeedbackDTOs;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.Evaluate
{
    public class FreelancerEvaluateModel : PageModel
    {
		private readonly HttpClient client = null;
		public FeebackDTO feedbackDTOs { get; set; } = default!;
		public int userId { get; set; }
		public string role { get; set; }
		public int JobID { get; set; }
		public int CreateByUserID { get; set; }


		public FreelancerEvaluateModel()
        {
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}
        public IActionResult OnGet(int jobId, int createByUserID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            this.JobID = jobId;
			this.CreateByUserID = createByUserID;
			if(HttpContext.Session.GetInt32("UserId")!= createByUserID)
			{
                return RedirectToPage("../Account/Login");
            }
			return Page();
        }
        public async Task<IActionResult> OnGetAddFeedback(int JobId, int CreateByUserID, int TargetToUserID, decimal Star, string Content)
		{
			//hmmm
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            role = HttpContext.Session.GetString("Role");


            var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = false,
			};
			string json = System.Text.Json.JsonSerializer.Serialize<CreateFeedbackDTO>(new CreateFeedbackDTO()
			{
				CreateByUserID = CreateByUserID,
				JobID = JobId,
				Star = Star,
				Content = Content
			}, options);
			StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Feedback/AddFeedback", httpContent);
			FeebackDTO x = new FeebackDTO();
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				x = JsonConvert.DeserializeObject<FeebackDTO>(strData);

			}
			return RedirectToPage("/Evaluate/FreelancerEvaluate");
		}
	}
}
