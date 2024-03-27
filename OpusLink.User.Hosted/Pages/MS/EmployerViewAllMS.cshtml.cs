using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.MS
{
    public class EmployerViewAllMSModel : PageModel
    {
        private readonly HttpClient client = null;
        public List<GetMilestoneResponse> milestones { get; set; } = default!;
        public int JobID { get; set; }
        public EmployerViewAllMSModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task OnGetAsync(int jobID)
        {
            //get all milestones of a job
            milestones = await GetAllMilestonesAsync(jobID);
        }

        private async Task<List<GetMilestoneResponse>> GetAllMilestonesAsync(int jobID)
        {
            JobID = jobID;
            //get all category
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/EMilestonesAPI/GetAllMilestone/"+ jobID);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetMilestoneResponse>>(strData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IActionResult> OnPostForAddAsync(IFormCollection collection)
        {
            CreateMilestoneRequest createMilestoneRequest = new CreateMilestoneRequest();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    createMilestoneRequest.JobID = Int32.Parse(collection[key]);
                }
                if (key.Contains("AddMilestoneContent"))
                {
                    createMilestoneRequest.MilestoneContent = collection[key];
                }
                if (key.Contains("AddDeadline"))
                {
                    createMilestoneRequest.Deadline = DateTime.ParseExact(collection[key], "dd-MM-yyyyTHH:mm", CultureInfo.InvariantCulture);
                }
                if (key.Contains("AddAmountToPay"))
                {
                    string amount = collection[key].ToString();
                    amount = amount.Replace(",", string.Empty);
                    amount = amount.Replace("₫", string.Empty);
                    amount = amount.Replace(" ", string.Empty);
                    createMilestoneRequest.AmountToPay = Decimal.Parse(amount);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateMilestoneRequest>(createMilestoneRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/EMilestonesAPI/AddMilestone", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
        }
    }
}
