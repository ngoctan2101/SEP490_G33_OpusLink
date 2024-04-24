using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Shared.Constants;
using System.Net.Http.Headers;
using System.Text.Json;


namespace OpusLink.User.Hosted.Pages.Freelancer.Profile
{
    public class ViewsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public UserDTO user { get; set; } = null!;
        public IList<SkillDTO> AllSkills { get; set; } = default!;
        public PutUserRequest PutUser { get; set; }
        public string Mess = "";
        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
        }
        public async Task<IActionResult> OnGetAsync(int UserId, string Mess)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            if (HttpContext.Session.GetInt32("UserIdCheck") == null)
            {
                HttpContext.Session.SetInt32("UserIdCheck", UserId);
            }

            // Kiểm tra nếu UserId không bằng UserId lưu trong Session thì chuyển hướng về trang với UserId ban đầu
            int originalUserId = HttpContext.Session.GetInt32("UserIdCheck") ?? 0;
            if (UserId != originalUserId)
            {
                HttpContext.Session.SetString("Notification", "Id sai hoặc bạn không có quyền truy cập");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
                return RedirectToPage("/Freelancer/Profile/Views", new { UserId = originalUserId });
            }

            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            this.Mess = Mess;
            if (HttpContext.Session.GetString("Role").Equals("Employer"))
            {
                return RedirectToPage("/Index");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetUserById/" + UserId);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
/*                HttpContext.Session.SetInt32("UserIdCheck", UserId);*/
            }
            else
            {
                return RedirectToPage("/Freelancer/Profile/Views", new { UserId = HttpContext.Session.GetInt32("UserIdCheck") });
            }
            //get all skill
            AllSkills = await GetAllSkillAsync();
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
        public async Task<ActionResult> OnPostForSaveAsync(IFormCollection collection, IFormFile image, IFormFile cv)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
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
            if (cv != null)
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
                else if (key.Contains("email"))
                {
                    PutUser.Email = collection[key];
                }
                else if (key.Contains("dob"))
                {
                    
                    try
                    {
                        PutUser.Dob = DateTime.Parse(collection[key]);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (key.Contains("phone"))
                {
                    PutUser.PhoneNumber = collection[key];
                }
                else if (key.Contains("address"))
                {
                    PutUser.Address = collection[key];
                }
                else if (key.Contains("userId"))
                {
                    PutUser.Id = Int32.Parse(collection[key]);
                }
                else if (key.Contains("bankaccountinfor"))
                {
                    PutUser.BankAccountInfor = (collection[key]);
                }
                else if (key.Contains("bankname"))
                {
                    PutUser.BankName = collection[key];
                }

            }
            if (PutUser.Dob == null)
            {
                Mess = "Bạn cần nhập ngày sinh ";
                return RedirectToPage("/Freelancer/Profile/Views", new { UserId = PutUser.Id, Mess = Mess });

            }
            if (DateTime.Today.Year - PutUser.Dob.Value.Year < 18)
            {
                Mess = "Số tuổi phải hơn 18 tuổi ";
                return RedirectToPage("/Freelancer/Profile/Views", new { UserId = PutUser.Id , Mess = Mess });
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };

            string json = System.Text.Json.JsonSerializer.Serialize<PutUserRequest>(PutUser, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ServiceMangaUrl + "/User/PutUserUser", httpContent);
            if (response.IsSuccessStatusCode)
            {
                //message "User Edited" green
                HttpContext.Session.SetString("Notification", "Thông tin cá nhân đã được thay đổi");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
            }
            return RedirectToPage("/Freelancer/Profile/Views", new { UserId = PutUser.Id });
        }

    }
}
