using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;

namespace OpusLink.User.Hosted.Pages.VnPayment
{
    public class PaymentModel : PageModel
    {

        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        
        public PaymentModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
           
        }
        public void OnGet()
        {
        }
    }
}
