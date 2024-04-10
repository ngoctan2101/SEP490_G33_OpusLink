using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class EmployerUpdateJobPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public GetJobDetailResponse Job { get; set; } = default!;
        public PutJobRequest PutJob { get; set; }
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public IList<GetLocationResponse> AllLocations { get; set; } = default!;
        private int userId;

        public EmployerUpdateJobPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> OnGetAsync(int JobId)
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
            //get this Job
            Job=await GetJobDetail(JobId);
            return Page();
        }

        private async Task<GetJobDetailResponse> GetJobDetail(int jobId)
        {
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job15API/GetJobDetail/" + jobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
            }
            return null;
        }

        public async Task<RedirectToPageResult> OnPostAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            
                userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            
            PutJob = new PutJobRequest();
            PutJob.EmployerID = userId;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("title"))
                {
                    PutJob.JobTitle = collection[key];
                }
                else if (key.Contains("category"))
                {
                    PutJob.CategoryIDs.Add(Int32.Parse(collection[key]));
                }
                else if (key.Contains("content"))
                {
                    PutJob.JobContent = collection[key].ToString();
                }
                else if (key.Contains("budget_min"))
                {
                    PutJob.BudgetFrom = Decimal.Parse(collection[key].ToString());
                }
                else if (key.Contains("budget_max"))
                {
                    PutJob.BudgetTo = Decimal.Parse(collection[key].ToString());
                }
                else if (key.Contains("location"))
                {
                    PutJob.LocationID = Int32.Parse(collection[key].ToString());
                }
                else if (key.Contains("jobId"))
                {
                    PutJob.JobID = Int32.Parse(collection[key].ToString());
                }
                else if (key.Contains("freelancerId"))
                {
                    if (String.IsNullOrEmpty(collection[key].ToString()))
                    {
                        PutJob.FreelancerID = null;
                    }
                    else
                    {
                        PutJob.FreelancerID = Int32.Parse(collection[key].ToString());
                    }
                }
                else if (key.Contains("dateCreated"))
                {
                    
                    PutJob.DateCreated = DateTime.Parse(collection[key].ToString());
                }
                else if (key.Contains("status"))
                {
                    PutJob.Status = Int32.Parse(collection[key].ToString());
                }
            }
            PutJob.EndHiringDate = DateTime.Now.AddDays(7);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<PutJobRequest>(PutJob, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl+"/Job15API/EditJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                //message "Your job is requested" green
            }
            return RedirectToPage("/JOB/EmployerViewAllJobCreatedPage");
        }

        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job15API/GetAllCategory");
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
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job15API/GetAllLocation");
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
