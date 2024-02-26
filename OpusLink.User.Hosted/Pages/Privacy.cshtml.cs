using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;

namespace OpusLink.User.Hosted.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }
        public  void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            // chuyen ve trang home
            return RedirectToPage("/Index");
        }
    }

}
