using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Shared.Constants;

namespace OpusLink.User.Hosted.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        public string Email { get; set; }
        public ForgotPasswordModel() { }

        string link = UrlConstant.ApiBaseUrl+"/Account/forgotPassword?email=";
        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string email)
        {

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(link + email))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        ApiResponseModel apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(data);

                        if (apiResponse.IsSuccess)
                        {
                            ViewData["Error"] = apiResponse.Message;
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
