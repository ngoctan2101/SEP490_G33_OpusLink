using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpusLink.User.Hosted.Pages.Evaluate
{
    public class EmployerEvaluateModel : PageModel
    {
        public int JobID { get; set; }
        public int CreateByUserID { get; set; }
        public IActionResult OnGet(int jobId, int createByUserID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            this.JobID = jobId;
            this.CreateByUserID = createByUserID;
            return Page();
        }
    }
}
