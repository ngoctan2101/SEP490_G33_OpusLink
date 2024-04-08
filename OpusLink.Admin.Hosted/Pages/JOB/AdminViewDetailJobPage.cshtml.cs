using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.JOB
{
    public class AdminViewDetailJobPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public GetJobDetailResponse job { get; set; }
        public AdminViewDetailJobPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetAsync(int JobId)
        {
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job13API/GetJobDetail/" + JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
            }
        }
        public async Task<IActionResult> OnPostForDeleteAsync(IFormCollection collection)
        {
            int idOfJob = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("DeleteJobID"))
                {
                    idOfJob = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            HttpResponseMessage response =  await client.DeleteAsync(UrlConstant.ApiBaseUrl+"/Job13API/DeleteJob/" + idOfJob);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/JOB/AdminViewAllJobPage");
            }
            else
            {
                return RedirectToPage("/JOB/AdminViewAllJobPage");
            }
            
        }
    }
}
