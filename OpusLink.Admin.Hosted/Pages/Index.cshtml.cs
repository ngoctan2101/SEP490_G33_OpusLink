using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Shared.Constants;

namespace OpusLink.Admin.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /*        public void OnGet()
                {
                    HttpContext.Session.SetString("PageNow", "Index");
                }*/
        string linkLogOut = UrlConstant.ApiBaseUrl + "/Account/logout";

        public async Task<IActionResult> OnGetAsync()
        {

            string token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                return RedirectToPage("/Dashboard/DashboardAdmin",new { year=2024});
            }
        }

        public async Task<IActionResult> OnPostForLogOut()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(linkLogOut))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        ApiResponseModel apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(data);

                        if (apiResponse.IsSuccess)
                        {
                            HttpContext.Session.Clear();
                            HttpContext.Session.SetString("Notification", "Đăng xuất thành công");
                            HttpContext.Session.SetInt32("NotiIsNew", 1);
                            return RedirectToPage("/Account/Login");
                        }
                        else
                        {
                            ViewData["Error"] = apiResponse.Message;
                            return Page();
                        }
                    }
                }
            }
        }
    }
}
