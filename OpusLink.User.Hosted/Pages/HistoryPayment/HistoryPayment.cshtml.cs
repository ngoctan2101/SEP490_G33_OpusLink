using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.HistoryPayment
{
    public class HistoryPaymentModel : PageModel
    {
        [BindProperty]

        public List<HistoryPaymentDTO> his { get; set; } = null!;
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        public HistoryPaymentModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            //_validationService = validateService;
        }
        public async Task OnGetAsync(int UserId)
        {

            //int userId = 0;
            //int userId = 1;
            //if (HttpContext.Session.GetInt32("UserId") == null)
            //{
            //    return RedirectToPage("/Login_Register/Login");
            //}
            //else
            //{
            //    userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            //}
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"/HistoryPayment/GetHistoryPaymentByUserId/{UserId}");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                his = System.Text.Json.JsonSerializer.Deserialize<List<HistoryPaymentDTO>>(responseBodyUser, optionUser);
            }


        }
    }
}
