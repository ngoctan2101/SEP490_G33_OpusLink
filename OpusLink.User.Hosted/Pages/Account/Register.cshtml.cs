using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.ComponentModel.DataAnnotations;

namespace OpusLink.User.Hosted.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        string link = "https://localhost:7265/api/Account/register";
        public async Task<IActionResult> OnPostAsync(string username, string password, string email, string confirmPassword)
        {
            RegisterDTO account = new RegisterDTO()
            {
                UserName = username,
                Password = password,
                Email = email,
                ConfirmPassword = confirmPassword
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
                            ViewData["Success"] = apiResponse.Message;
                            TempData["Email"] = email;
                            return RedirectToPage("EmailVerify");
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