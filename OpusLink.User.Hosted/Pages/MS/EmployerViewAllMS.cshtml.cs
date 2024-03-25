using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System.Net.Http.Headers;

namespace OpusLink.User.Hosted.Pages.MS
{
    public class EmployerViewAllMSModel : PageModel
    {
        private readonly HttpClient client = null;
        public List<Milestone> milestones { get; set; } = default!;
        public EmployerViewAllMSModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task OnGetAsync()
        {
            //get all milestones of a job
            milestones = await GetAllMilestonesAsync();
        }

        private Task<List<Milestone>> GetAllMilestonesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
