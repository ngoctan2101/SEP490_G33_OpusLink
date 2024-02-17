using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.Models.JOB;
using System.Net.Http.Headers;

namespace OpusLink.Admin.Hosted.Pages.JOB
{
    public class AdminViewAllJobPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<GetJobResponse> Jobs { get; set; } = default!;
        public IList<GetCategoryResponse> Categories { get; set; } = default!;
        public Filter filter { get; set; }

        //will add more for usecase search and filter

        public AdminViewAllJobPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync()
        {
            //get all jobs
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job3API/GetAllJob");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Jobs = JsonConvert.DeserializeObject<List<GetJobResponse>>(strData);
            }
            else
            {

            }
            //get all category has parent is null (0)
            response = await client.GetAsync("https://localhost:7265/api/Job3API/GetAllChildCategory?parentId=" + 0);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
            }
            else
            {

            }
        }
    }
}
