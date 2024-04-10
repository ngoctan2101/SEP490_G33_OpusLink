using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<IActionResult> OnGetAsync()
        {

            string token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                return RedirectToPage("/Dashboard/DashboardAdmin");
            }
        }
    }
}
