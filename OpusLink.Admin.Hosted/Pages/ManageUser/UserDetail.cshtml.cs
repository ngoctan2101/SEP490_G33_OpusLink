using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Http;
using OpusLink.Entity.Models;

namespace OpusLink.Admin.Hosted.Pages.ManageUser
{
    public class UserDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public UserDTO user { get; set; } = null!;
        public IList<SkillDTO> AllSkills { get; set; } = default!;
        public PutUserRequest PutUser { get; set; }
        public UserDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
        }
        public async Task OnGetAsync(int id)
        {
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetUserById/"+id);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }
            //get all skill
            AllSkills = await GetAllSkillAsync();
        }
        private async Task<IList<SkillDTO>> GetAllSkillAsync()
        {
            //get all skill
            HttpResponseMessage response = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetAllSkill");
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
            int userId = UserId;
            HttpResponseMessage response = await client.GetAsync(ServiceMangaUrl + "api/User/GetFileCVById/"+userId);

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
        public async Task<ActionResult> OnPostForSaveAsync(IFormCollection collection, IFormFile image, IFormFile cv)
        {
            PutUser = new PutUserRequest();
            //get image, get cv from <input>
            if (image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    PutUser.UserImageBytes = memoryStream.ToArray();
                }
                PutUser.imageExtension = Path.GetExtension(image.FileName).ToLower();
            }
            if(cv!=null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await cv.CopyToAsync(memoryStream);
                    PutUser.UserCVBytes = memoryStream.ToArray();
                }
                PutUser.cvExtension = Path.GetExtension(cv.FileName).ToLower();
            }
            
            
            // manual bind to get UserDTO object
            List<string> keys = collection.Keys.ToList<string>();
            foreach (string key in keys)
            {
                if (key.Contains("introduction"))
                {
                    PutUser.Introduction = collection[key];
                }
                else if (key.Contains("skill"))
                {
                    PutUser.SkillIDs.Add(Int32.Parse(collection[key]));
                }
                else if (key.Contains("userId"))
                {
                    PutUser.Id=Int32.Parse(collection[key]);
                }
                
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            PutUser.Email = "caicut@123";
            string json = System.Text.Json.JsonSerializer.Serialize<PutUserRequest>(PutUser, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ServiceMangaUrl + "api/User/PutUserAdmin", httpContent);
            if (response.IsSuccessStatusCode)
            {
                //message "User Edited" green
            }
            return RedirectToPage("/ManageUser/UserDetail", new { id = PutUser.Id });
        }
    }
}
