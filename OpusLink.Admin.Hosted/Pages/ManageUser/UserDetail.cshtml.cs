using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.ManageUser
{
    public class UserDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public UserDTO user { get; set; } = null!;
        //public int SkillID { get; set; }
        //public int? SkillParentID { get; set; }
        //public string SkillName { get; set; }
        public UserDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
        }
        public async Task OnGetAsync(int id)
        {
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetUserById/"+id);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }
        }
    }
}
