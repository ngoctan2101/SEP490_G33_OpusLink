using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using System.IdentityModel.Tokens.Jwt;

namespace OpusLink.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        // ----LINK API----
        string linkUpdateRole = "https://localhost:7265/api/Account/update-role";
        string linkLogOut = "https://localhost:7265/api/Account/logout";

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
                            string currentRole = jsonToken.Claims.First(claim => claim.Type == "currentRole").Value;
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
    }
}
