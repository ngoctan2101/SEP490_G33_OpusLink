using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpusLink.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string role)
        {
            if (String.IsNullOrEmpty(role))
            {
                role = "Freelancer";
            }
            if (role.Equals("Freelancer"))
            {
                HttpContext.Session.SetString("Role", "Freelancer");
            }
            else if (role.Equals("Employer"))
            {
                HttpContext.Session.SetString("Role", "Employer");
            }
        }
    }
}
