using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using System.Net.Http.Headers;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class EmployerViewJobDetailPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public GetJobDetailResponse job { get; set; }
        public List<GetOfferAndFreelancerResponse> offers { get; set; } 
        public EmployerViewJobDetailPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync(int JobId)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job15API/GetJobDetail/" + JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
            }
            //get list offers for job
            response = await client.GetAsync("https://localhost:7265/api/Offer3API/GetAllOfferOfJob/" + JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                offers = JsonConvert.DeserializeObject<List<GetOfferAndFreelancerResponse>>(strData);
            }
        }
    }
}
