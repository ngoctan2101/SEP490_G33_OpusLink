using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Shared.VnPay;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using OpusLink.Entity.Models;

namespace OpusLink.User.Hosted.Pages.HistoryPayment
{
    public class HistoryPaymentDetailModel : PageModel
    {
        [BindProperty]
        
        public HistoryPaymentDTO his { get; set; } = null!;
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        public HistoryPaymentDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            //_validationService = validateService;
        }
        public async Task OnGetAsync(int payId)
        {
            
            //int userId = 0;
            int userId = 1;
            //if (HttpContext.Session.GetInt32("UserId") == null)
            //{
            //    return RedirectToPage("/Login_Register/Login");
            //}
            //else
            //{
            //    userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            //}
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"api/HistoryPayment/GetHistoryPaymentById/{payId}");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                his = JsonSerializer.Deserialize<HistoryPaymentDTO>(responseBodyUser, optionUser);
            }
         

        }
    }
}
