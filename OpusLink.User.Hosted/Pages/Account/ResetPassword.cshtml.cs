using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Shared.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace OpusLink.User.Hosted.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        public string Password { get; set; }
        public ResetPasswordModel() { }

        string link = UrlConstant.ApiBaseUrl+"/Account/resetPassword";

        public async Task<IActionResult> OnGetAsync(string token, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(link + "?token=" + Uri.EscapeDataString(token) + "&email=" + email))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        ApiResponseModel apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(data);
                        // Example return statement
                        HttpContext.Session.SetString("token", token);
                        HttpContext.Session.SetString("email", email);
                        return Page();
                    }
                }
            }
        }
        public async Task<IActionResult> OnPostAsync(string password)
        {
            string email = HttpContext.Session.GetString("email");
            string token = HttpContext.Session.GetString("token");

            ResetPassword account = new ResetPassword()
            {
                Password = password,
                Email = email,
                Token = token
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
                            return RedirectToPage("Login");
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