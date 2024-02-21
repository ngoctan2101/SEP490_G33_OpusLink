using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.ManageUser
{
    public class ViewsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public List<UserDTO> listUser { get; set; } = null!;
        //public int SkillID { get; set; }
        //public int? SkillParentID { get; set; }
        //public string SkillName { get; set; }
        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
        }
        
        public async Task OnGetAsync()
        {
            // call list
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetAllUser");
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listUser = JsonSerializer.Deserialize<List<UserDTO>>(responseBodyUser, optionUser);
            }
        }
    }
}
