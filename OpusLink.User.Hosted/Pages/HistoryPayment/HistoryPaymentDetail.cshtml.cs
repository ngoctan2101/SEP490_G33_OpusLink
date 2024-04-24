using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Shared.VnPay;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;

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
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            //_validationService = validateService;
        }
        public async  Task<IActionResult> OnGetAsync(int payId)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            if (HttpContext.Session.GetInt32("PayIdCheck") == null)
            {
                HttpContext.Session.SetInt32("PayIdCheck", payId);
            }

            // Kiểm tra nếu UserId không bằng UserId lưu trong Session thì chuyển hướng về trang với UserId ban đầu
            int originalPayId = HttpContext.Session.GetInt32("PayIdCheck") ?? 0;
            if (payId != originalPayId)
            {
                HttpContext.Session.SetString("Notification", "Id sai hoặc bạn không có quyền truy cập");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
                return RedirectToPage("/HistoryPayment/HistoryPaymentDetail", new { payId = originalPayId });
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            int userId = 0;
            userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"/HistoryPayment/GetHistoryPaymentById/{payId}");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                his = JsonSerializer.Deserialize<HistoryPaymentDTO>(responseBodyUser, optionUser);
            }
            else
            {
                return RedirectToPage("/HistoryPayment/HistoryPaymentDetail", new { UserId = HttpContext.Session.GetInt32("PayIdCheck") });
            }

            return Page();
        }
    }
}
