using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.IdentityModel.Tokens.Jwt;

namespace OpusLink.User.Hosted.Pages.Account
{
    public class LoginModel : PageModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginModel() { }

        string link = "https://localhost:7265/api/Account/Login";
        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            LoginDTO account = new LoginDTO()
            {
                UserName = username,
                Password = password
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
                            return RedirectToPage("/Index");
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
