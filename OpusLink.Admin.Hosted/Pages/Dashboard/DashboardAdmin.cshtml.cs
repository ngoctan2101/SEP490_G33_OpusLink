using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.AutoMapper.JOB;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
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
        [BindProperty]
        public List<UserDTO> listUser { get; set; } = null!;
        public List<GetJobResponse> listJob { get; set; } = null!;
        //public int SkillID { get; set; }
        //public int? SkillParentID { get; set; }
        //public string SkillName { get; set; }
        public DashboardAdminModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
        }

        public async Task OnGetAsync()
        {
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetAllUser");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listUser = JsonSerializer.Deserialize<List<UserDTO>>(responseBodyUser, optionUser);
                TotalUsers = listUser.Count;
                
            }
           
            responseUser = await client.GetAsync(ServiceMangaUrl + "api/Job3API/GetAllJob2");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listJob = JsonSerializer.Deserialize<List<GetJobResponse>>(responseBodyUser, optionUser);
                
                TotalJobs = listJob.Count;
            }
        }
    }

}
