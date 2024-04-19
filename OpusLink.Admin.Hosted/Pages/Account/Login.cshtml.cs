using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.IdentityModel.Tokens.Jwt;
using OpusLink.Shared.Constants;

namespace OpusLink.Admin.Hosted.Pages.Account
{
    public class LoginModel : PageModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginModel() { }

        string link = UrlConstant.ApiBaseUrl+ "/AdminAccount/loginAdmin";
        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            HttpContext.Session.Clear();
            if (!IsPasswordValid(password))
            {
                ViewData["Error"] = TotalMessage.LoginError;
                return Page();
            }

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
                            string userId = jsonToken.Claims.First(claim => claim.Type == "userId").Value;

                            HttpContext.Session.SetInt32("UserId", Int32.Parse(userId));
                            HttpContext.Session.SetString("token", token);
                            HttpContext.Session.SetString("Notification", "Đăng nhập thành công");
                            HttpContext.Session.SetInt32("NotiIsNew", 1);

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
        public bool IsPasswordValid(string password)
        {
            if (password.Length < 6 ||
                !password.Any(IsSpecialChar) ||
                !password.Any(char.IsDigit) ||
                (!password.Any(char.IsUpper) || !password.Any(char.IsLower)))
            {
                return false;
            }
            return true;
        }

        public static bool IsSpecialChar(char c)
        {
            return !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c);
        }
    }
}
