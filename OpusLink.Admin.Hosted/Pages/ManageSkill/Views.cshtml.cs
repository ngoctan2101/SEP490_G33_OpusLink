using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.ManageSkill
{
    public class ViewsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public List<SkillDTO> listSkills { get; set; } = null!;
        public SkillDTO SkillDto { get; set; } = null!;
       
        public IList<SkillDTO> AllSkills { get; set; } = default!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            PageNo = 1;
            //category thi chi can moi search thoi
            filter = new Filter()
            {
                SearchStr = ""
            };
        }
        // list skill
        public async Task OnGetAsync()
        {
            HttpContext.Session.SetString("PageNow", "ManageSkill");
            // call list
            //HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetAllSkill");
            //if (responseSkill.IsSuccessStatusCode)
            //{
            //    string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
            //    var optionSkill = new JsonSerializerOptions()
            //    { PropertyNameCaseInsensitive = true };
            //    listSkills = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            //}
            //get list first 10 skills
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseSkill = await client.PostAsync(ServiceMangaUrl + "api/Skill/GetTenSkill", httpContent);

            // Check if the HTTP request was successful
            if (responseSkill.IsSuccessStatusCode)
            {
                // Read the response body as a string
                string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();

                // Configure JsonSerializer options (ignoring property case)
                var optionSkill = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialize the JSON response into a List of SkillDTO objects
                listSkills = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = listSkills.ElementAt(listSkills.Count - 1).SkillID;
                listSkills.RemoveAt(listSkills.Count - 1);
            }
            //return list ALL skill (for add,edit)
            AllSkills = await GetAllSkillAsync();
        }

        private async Task<IList<SkillDTO>> GetAllSkillAsync()
        {
            HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetAllSkill");

            // Check if the HTTP request was successful
            if (responseSkill.IsSuccessStatusCode)
            {
                // Read the response body as a string
                string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();

                // Configure JsonSerializer options (ignoring property case)
                var optionSkill = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialize the JSON response into a List of SkillDTO objects
                return JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            }
            else
            {
                return null;
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
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + "api/Skill/GetTenSkill", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string responseBodySkill = await response.Content.ReadAsStringAsync();
                listSkills = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, options);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = listSkills.ElementAt(listSkills.Count - 1).SkillID;
                listSkills.RemoveAt(listSkills.Count - 1);
            }
            else
            {

            }
            //return list ALL skill (for add,edit)
            AllSkills = await GetAllSkillAsync();
        }
        //public async Task OnGetAsync(int SkillId)
        //{

        //    HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetSkillById" + SkillId);
        //    if (responseSkill.IsSuccessStatusCode)
        //    {
        //        string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
        //        var optionSkill = new JsonSerializerOptions()
        //        { PropertyNameCaseInsensitive = true };
        //        SkillDto = JsonSerializer.Deserialize<SkillDTO>(responseBodySkill, optionSkill);
        //    }
        //}
        //public async Task OnPostAsync(Skill newSkill)
        //{
        //    var optionsAddSkill = new JsonSerializerOptions()
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    string jsonRequestBody = JsonSerializer.Serialize(newSkill, optionsAddSkill);

        //    StringContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

        //    HttpResponseMessage responseAddSkill = await client.PostAsync(ServiceMangaUrl + "api/Skill/AddSkill", content);

        //    if (responseAddSkill.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine("Skill added successfully.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to add skill. Status code: {responseAddSkill.StatusCode}");
        //    }
        //}

        // add new skill
        public async Task<IActionResult> OnPostAsync(SkillDTO newSkill)
        {

            var jsonRequestBody = JsonSerializer.Serialize(newSkill, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ServiceMangaUrl + "api/Skill/AddSkill", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Views"); // Redirect to the skills index page after successful creation
            }
            else
            {
                // Handle error
                return Page();
            }
        }


        //public async Task OnPutAsync(Skill existingSkill)
        //{
        //    var optionsUpdateSkill = new JsonSerializerOptions()
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    string jsonRequestBody = JsonSerializer.Serialize(existingSkill, optionsUpdateSkill);

        //    StringContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

        //    HttpResponseMessage responseUpdateSkill = await client.PutAsync(ServiceMangaUrl + $"api/Skill/UpdateSkill/{existingSkill.SkillID}", content);

        //    if (responseUpdateSkill.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine("Skill updated successfully.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to update skill. Status code: {responseUpdateSkill.StatusCode}");
        //    }
        //}

        // update skill
        //[HttpPost("{SkillId:int?}")]
        public async Task<IActionResult> OnPostForUpdateAsync(SkillDTO updatedSkill)
        {


            var jsonRequestBody = JsonSerializer.Serialize(updatedSkill, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            // Assuming your API endpoint for updating a skill is using the PUT method
            var response = await client.PutAsync(ServiceMangaUrl + $"api/Skill/UpdateSkill", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Views"); // Redirect to the skills index page after successful update
            }
            else
            {
                // Handle error
                return Page();
            }

        }

        public async Task DeleteSkillAsync(int skillIdToDelete)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseDeleteSkill = await client.DeleteAsync(ServiceMangaUrl + $"api/Skill/DeleteSkillById/{skillIdToDelete}");

                if (responseDeleteSkill.IsSuccessStatusCode)
                {
                    Console.WriteLine("Skill deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to delete skill. Status code: {responseDeleteSkill.StatusCode}");
                }
            }
        }
    }
}
