
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.AutoMapper.JOB;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.Dashboard
{
    public class DashboardAdminModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        public int TotalUsers { get; set; }
        public int TotalJobs { get; set; }
        public int TotalJobsRequest { get; set; }
        [BindProperty]
        public List<UserDTO> listUser { get; set; } = null!;
        public List<ChatDTO> chats { get; set; } = null!;
        public List<GetJobResponse> listJob { get; set; } = null!;
        public List<GetJobResponse> Jobs { get; set; } = default!;
        //public int SkillID { get; set; }
        //public int? SkillParentID { get; set; }
        //public string SkillName { get; set; }
        public DashboardAdminModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
        }

        public async Task OnGetAsync()
        {
            HttpContext.Session.SetString("PageNow", "Dashboard");
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetAllUser");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listUser = JsonSerializer.Deserialize<List<UserDTO>>(responseBodyUser, optionUser);
                TotalUsers = listUser.Count;

            }

            responseUser = await client.GetAsync(ServiceMangaUrl + "/Job3API/GetAllJob2");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listJob = JsonSerializer.Deserialize<List<GetJobResponse>>(responseBodyUser, optionUser);

                TotalJobs = listJob.Count;
            }
            HttpResponseMessage responseJobRequest = await client.GetAsync(ServiceMangaUrl + "/Job12API/GetAllJobRequested2");
            if (responseJobRequest.IsSuccessStatusCode)
            {
                string responseBodyJobRequest = await responseJobRequest.Content.ReadAsStringAsync();
                var optionJobRequest = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                var jobRequests = JsonSerializer.Deserialize<List<GetJobResponse>>(responseBodyJobRequest, optionJobRequest);
                TotalJobsRequest = jobRequests.ElementAt(jobRequests.Count-1).EmployerID;
                jobRequests.RemoveAt(jobRequests.Count - 1);
            }

        }
    }

}