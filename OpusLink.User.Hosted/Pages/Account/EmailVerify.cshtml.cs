using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace OpusLink.User.Hosted.Pages.Account
{
    public class EmailVerifyModel : PageModel
    {
        public string Email { get; set; }
/*        public void OnGet(string token)
        {
            Email = TempData["Email"] as string;
        }*/
        string link = "https://localhost:7265/api/Account/confirmEmail?token=";

        public async Task<IActionResult> OnGetAsync(string token, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                if (token == null)
                {
                    Email = TempData["Email"] as string;
                    return Page();
                }
                else
                {
                    //UrltoRedirect
                    using (HttpResponseMessage response = await client.GetAsync(link + Uri.EscapeDataString(token) + "&email=" + email))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            ApiResponseModel apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(data);

                            if (apiResponse.IsSuccess)
                            {
                                ViewData["Message"] = apiResponse.Message;
                                return RedirectToPage("Login");

                            }
                            else
                            {
                                ViewData["Message"] = apiResponse.Message;
                                return Page();
                            }
                        }
                    }
                }
            }
        }
    }
}
