using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class EmployerCreateAJobPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public CreateJobRequest Job { get; set; } = default!;
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public IList<GetLocationResponse> AllLocations { get; set; } = default!;

        public EmployerCreateAJobPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task OnGetAsync()
        {
            //get all category
            AllCategories = await GetAllCategoryAsync();
            //get all location
            AllLocations = await GetAllLocationAsync();
        }

        

        public async Task<RedirectToPageResult> OnPostAsync(IFormCollection collection)
        {
            Job = new CreateJobRequest();
            Job.EmployerId = 20;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("title"))
                {
                    Job.JobTitle = collection[key];
                }
                else if (key.Contains("category"))
                {
                   Job.CategoryIds.Add(Int32.Parse(collection[key]));
                }
                else if (key.Contains("content"))
                {
                    Job.JobContent=collection[key].ToString();
                }
                else if (key.Contains("budget_min"))
                {
                    Job.BudgetMin = Decimal.Parse(collection[key].ToString());
                }
                else if (key.Contains("budget_max"))
                {
                    Job.BudgetMax = Decimal.Parse(collection[key].ToString());
                }
                else if (key.Contains("location"))
                {
                    Job.LocationId = Int32.Parse(collection[key].ToString());
                }
            }
            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateJobRequest>(Job, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job10API/CreateJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                //message "Your job is requested" green
            }
            return RedirectToPage("/JOB/EmployerViewAllJobCreatedPage");
        }

        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job10API/GetAllCategory");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
            }
            else
            {
                return null;
            }
        }
        private async Task<IList<GetLocationResponse>> GetAllLocationAsync()
        {
            //get all location
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job10API/GetAllLocation");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetLocationResponse>>(strData);
            }
            else
            {
                return null;
            }
        }
    }
}
