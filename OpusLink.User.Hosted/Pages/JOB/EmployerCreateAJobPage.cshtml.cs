using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
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
        private int userId;

        public EmployerCreateAJobPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            //get all category
            AllCategories = await GetAllCategoryAsync();
            //get all location
            AllLocations = await GetAllLocationAsync();
            return Page();
        }

        

        public async Task<RedirectToPageResult> OnPostAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            userId = HttpContext.Session.GetInt32("UserId")??0;
            
            Job = new CreateJobRequest();
            Job.EmployerId = userId;
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
            Job.EndHiringDate = DateTime.Now.AddDays(7);
            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateJobRequest>(Job, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Job10API/CreateJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Notification", "Yêu cầu đăng bài của bạn sẽ được hệ thống xử lý trong vòng 48h tới");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
            }
            return RedirectToPage("/JOB/EmployerViewAllJobCreatedPage");
        }


        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job10API/GetAllCategory");
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
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job10API/GetAllLocation");
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
