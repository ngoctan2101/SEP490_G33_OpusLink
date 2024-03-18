using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace OpusLink.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        string link = "https://localhost:7265/api/Account/update-role";
        string linkLogOut = "https://localhost:7265/api/Account/logout";

        public async Task<IActionResult> OnGetAsync(string role)
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
            UpdateRoleDTO account = new UpdateRoleDTO()
            {
                UserName = HttpContext.Session.GetString("userName"),
                CurrentRole = HttpContext.Session.GetString("currentRole"),
            };
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsJsonAsync(link, account))
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

                            // Lấy ra UserId từ claims
                            string userId = jsonToken.Claims.First(claim => claim.Type == "UserId").Value;
                            string currentRole = jsonToken.Claims.First(claim => claim.Type == "role").Value;
                            string name = jsonToken.Claims.First(claim => claim.Type == "unique_name").Value;

                            HttpContext.Session.SetInt32("UserId", Int32.Parse(userId));

                            HttpContext.Session.SetString("token", token);
                            HttpContext.Session.SetString("currentRole", currentRole);
                            HttpContext.Session.SetString("userName", name);

                            /*return RedirectToPage("/Index", new { token = apiResponse.Data.ToString() });*/
                            return Page();
                        }
                        else
                        {
                            if (apiResponse.Code == 0)
                            {
                                ViewData["Error"] = TotalMessage.LoginError;
                                return Page();
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

        public async Task<IActionResult> OnGetForLogOut()
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

                            /*return RedirectToPage("/Index", new { token = apiResponse.Data.ToString() });*/
                            return Page();
                        }
                        else
                        {
                            if (apiResponse.Code == 0)
                            {
                                ViewData["Error"] = TotalMessage.LoginError;
                                return Page();
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
}
