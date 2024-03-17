using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class FreelancerViewAllJobSavedPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<GetSaveJobResponse> SavedJobs { get; set; } = default!;
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public int userID { get; set; }

        public FreelancerViewAllJobSavedPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PageNo = 1;
            filter = new Filter()
            {
                SearchStr = "",
                BudgetMin = 100000,
                BudgetMax = 500000000,
                DateMin = DateTime.ParseExact("2023-01-01 00:01", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateMax = DateTime.ParseExact("2024-04-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                userID = HttpContext.Session.GetInt32("UserId") ?? 0;
            }
            //Get All Offer by userID
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job6API/GetAllJobSaved/" + userID, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                SavedJobs = JsonConvert.DeserializeObject<List<GetSaveJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = SavedJobs.ElementAt(SavedJobs.Count - 1).SaveJobID;
                SavedJobs.RemoveAt(SavedJobs.Count - 1);
            }
            //get all category
            AllCategories = await GetAllCategoryAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostForSearchAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                userID = HttpContext.Session.GetInt32("UserId") ?? 0;
            }
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                else if (key.Contains("pageNo"))
                {
                    filter.PageNumber = Int32.Parse(collection[key]);
                    PageNo = filter.PageNumber;
                }
            }
            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job6API/GetAllJobSaved/" + userID, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                SavedJobs = JsonConvert.DeserializeObject<List<GetSaveJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = SavedJobs.ElementAt(SavedJobs.Count - 1).SaveJobID;
                SavedJobs.RemoveAt(SavedJobs.Count - 1);
            }
            //get all category
            AllCategories = await GetAllCategoryAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostForUnSaveAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                userID = HttpContext.Session.GetInt32("UserId") ?? 0;
            }
            int SaveJobId = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                else if (key.Contains("btnUnSave"))
                {
                    SaveJobId = Int32.Parse(collection[key]);
                }
            }

            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            //unsave it
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:7265/api/Job7API/DeleteSaveJob/" + SaveJobId);
            if (response.IsSuccessStatusCode)
            { }
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync("https://localhost:7265/api/Job6API/GetAllJobSaved/" + userID, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                SavedJobs = JsonConvert.DeserializeObject<List<GetSaveJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = SavedJobs.ElementAt(SavedJobs.Count - 1).SaveJobID;
                SavedJobs.RemoveAt(SavedJobs.Count - 1);
            }
            //get all category
            AllCategories = await GetAllCategoryAsync();
            return Page();
        }

        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job6API/GetAllCategory");
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
    }
}
