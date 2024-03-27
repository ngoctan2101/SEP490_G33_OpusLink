using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;

namespace OpusLink.Admin.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        string linkLogOut = "https://localhost:7265/api/Account/logout";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

/*        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.SetString("PageNow", "Index");
            return RedirectToPage("Login");
        }*/

        public async Task<IActionResult> OnGetAsync()
        {
            string currentRole = HttpContext.Session.GetString("Role");
            if(currentRole == "Admin")
            {
                return Page();
            }
            else
            {
                HttpContext.Session.SetString("PageNow", "Index");
                return RedirectToPage("/Account/Login");
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
