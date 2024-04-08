using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Shared.Constants;
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
        public List<SkillDTO> listSkill { get; set; } = null!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            PageNo = 1;
            //user thi chi can moi search thoi
            filter = new Filter()
            {
                SearchStr = ""
            };
        }
        
        public async Task OnGetAsync()
        {
            HttpContext.Session.SetString("PageNow", "ManageUser");
            //get list 10 skill base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseUser = await client.PostAsync(ServiceMangaUrl + "/User/GetTenUser", httpContent);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listUser = JsonSerializer.Deserialize<List<UserDTO>>(responseBodyUser, optionUser);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = listUser.ElementAt(listUser.Count - 1).Id;
                listUser.RemoveAt(listUser.Count - 1);
            }




            //return list ALL skill (for filter ?)
            HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "/Skill/GetAllSkill");
            if (responseSkill.IsSuccessStatusCode)
            {
                string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
                var optionSkill = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listSkill = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            }
        }
        public async Task OnPostForSearchAsync(IFormCollection collection)
        {
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                else if (key.Contains("pageNo"))
                {
                    filter.PageNumber = Int32.Parse(collection[key]);
                    PageNo = filter.PageNumber;
                }
            }
            //get list 10 skill base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + "/User/GetTenUser", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBodySkill = await response.Content.ReadAsStringAsync();
                listUser = JsonSerializer.Deserialize<List<UserDTO>>(responseBodySkill, options);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = listUser.ElementAt(listUser.Count - 1).Id;
                listUser.RemoveAt(listUser.Count - 1);
            }
            else
            {

            }
            //return list ALL skill (for filter ?)
            HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "/Skill/GetAllSkill");
            if (responseSkill.IsSuccessStatusCode)
            {
                string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
                var optionSkill = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listSkill = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            }
        }
    }
}
