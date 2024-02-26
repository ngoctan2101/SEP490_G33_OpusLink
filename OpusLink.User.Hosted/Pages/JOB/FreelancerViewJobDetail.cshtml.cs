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
    public class FreelancerViewJobDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        public GetJobDetailResponse job { get; set; }
        public FreelancerViewJobDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync(int JobId)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job5API/GetJobDetail/"+JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
            }
        }
    }
}
