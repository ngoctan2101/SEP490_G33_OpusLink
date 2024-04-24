using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.Freelancer.Profile
{
    public class ViewProfileOtherFreelancerModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public UserDTO user { get; set; } = null!;
        //public int SkillID { get; set; }
        //public int? SkillParentID { get; set; }
        //public string SkillName { get; set; }
        public ViewProfileOtherFreelancerModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
        }
        public async Task<IActionResult> OnGetAsync(int UserId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetUserById/" + UserId);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
                HttpContext.Session.SetInt32("UserIdCheckNew", UserId);
            }
            else
            {
                return RedirectToPage("/Freelancer/Profile/ViewsProfileOtherFreelancer", new { UserId = HttpContext.Session.GetInt32("UserIdCheckNew") });
            }
            return Page();
        }
        private async Task<IList<SkillDTO>> GetAllSkillAsync()
        {
            //get all skill
            HttpResponseMessage response = await client.GetAsync(ServiceMangaUrl + "/Skill/GetAllSkill");
            if (response.IsSuccessStatusCode)
            {
                string responseBodyUser = await response.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SkillDTO>>(responseBodyUser, optionUser);
            }
            else
            {
                return null;
            }
        }
        public async Task<ActionResult> OnGetForDownloadAsync(int UserId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            int userId = UserId;
            HttpResponseMessage response = await client.GetAsync(ServiceMangaUrl + "/User/GetFileCVById/" + userId);

            if (response.IsSuccessStatusCode)
            {
                // Read the file content as a byte array
                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                // Get the content type from the response headers
                string contentType = response.Content.Headers.ContentType.ToString();

                if (contentType == "application/pdf")
                {
                    // Return the file as a FileResult
                    return File(fileBytes, contentType, "resume.pdf");
                }
                else if (contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    // Return the file as a FileResult
                    return File(fileBytes, contentType, "resume.docx");
                }
            }
            // Handle the case where the request was not successful
            return Content("Error downloading file");
        }
    }
}
