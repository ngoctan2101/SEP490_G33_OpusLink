using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using OpusLink.Entity.DTO.NotificationDTO;
using OpusLink.Entity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpusLink.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        // ----LINK API----
        string linkUpdateRole = "https://localhost:7265/api/Account/update-role";
        string linkLogOut = "https://localhost:7265/api/Account/logout";
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            //_validationService = validateService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string currentRole = HttpContext.Session.GetString("Role");
            if (currentRole == "Employer")
            {
                return RedirectToPage("/JOB/EmployerViewAllJobCreatedPage");
            }
            else
            {
                return RedirectToPage("/JOB/FreelancerViewAllJobPage");
            }
        }
        string link = "https://localhost:7265/api/Account/update-role";
       

        public async Task<IActionResult> OnPostChangeTokenAboutRole()
        {
            UpdateRoleDTO account = new UpdateRoleDTO()
            {
                UserName = HttpContext.Session.GetString("userName"),
                CurrentRole = HttpContext.Session.GetString("Role")
            };

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsJsonAsync(linkUpdateRole, account))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        ApiResponseModel apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(data);

                        if (apiResponse.IsSuccess)
                        {
                            string token = apiResponse.Data.ToString();
                            var handler = new JwtSecurityTokenHandler();
                            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                            // Lấy thông tin từ Claims
                            string userId = jsonToken.Claims.First(claim => claim.Type == "userId").Value;
                            string currentRole = jsonToken.Claims.First(claim => claim.Type == "role").Value;
                            string name = jsonToken.Claims.First(claim => claim.Type == "userName").Value;

                            //Add thông tin lấy được vào Session
                            HttpContext.Session.SetInt32("UserId", Int32.Parse(userId));
                            HttpContext.Session.SetString("token", token);
                            HttpContext.Session.SetString("Role", currentRole);
                            HttpContext.Session.SetString("userName", name);

                            //Trả lại Page Index
                            if(currentRole == "Freelancer")
                            {
                                return RedirectToPage("/JOB/FreelancerViewAllJobPage");
                            }
                            else
                            {
                                return RedirectToPage("/JOB/EmployerViewAllJobCreatedPage");
                            }
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
                            return RedirectToPage("/JOB/FreelancerViewAllJobPage");
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

		public async Task<IActionResult> OnGetForNotificationDetailAsync(string link,int notiId)
		{
            // update readed
            var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(notiId, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ServiceMangaUrl + $"api/Notification/UpdateNotificationReader/{notiId}", content);
           
            return CreateRedirectToPage(link);



        }
        // withdrawrequest 
        public static RedirectToPageResult CreateRedirectToPage(string originalUrl)
        {
            Uri x = new Uri(originalUrl);

            // Parse UserId and UserName from the original URL using regular expressions
            Regex userIdRegex = new Regex(@"UserId=(\d+)");
            //Regex userNameRegex = new Regex(@"UserName=([^&]+)");

            Match userIdMatch = userIdRegex.Match(originalUrl);
            //Match userNameMatch = userNameRegex.Match(originalUrl);

            int userId = 0;
            //string userName = "";

            if (userIdMatch.Success)
            {
                userId = int.Parse(userIdMatch.Groups[1].Value);
            }

            //if (userNameMatch.Success)
            //{
            //    userName = userNameMatch.Groups[1].Value;
            //}

            // Construct RedirectToPage object
            return new RedirectToPageResult(x.AbsolutePath, new { UserId = userId});
        }

    }
}
