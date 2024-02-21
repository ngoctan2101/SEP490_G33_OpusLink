using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
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
        // list skill
        public async Task OnGetAsync()
        {
            // call list
            //HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetAllSkill");
            //if (responseSkill.IsSuccessStatusCode)
            //{
            //    string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
            //    var optionSkill = new JsonSerializerOptions()
            //    { PropertyNameCaseInsensitive = true };
            //    listSkills = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            //}

            

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
                listSkills = JsonSerializer.Deserialize<List<SkillDTO>>(responseBodySkill, optionSkill);
            }
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

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
        [HttpPost("{SkillId:int?}")]
        public async Task<IActionResult> OnPutAsync(Skill updatedSkill, int SkillId)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                HttpResponseMessage responseSkill = await client.GetAsync(ServiceMangaUrl + "api/Skill/GetSkillById" + SkillId);
                if (responseSkill.IsSuccessStatusCode)
                {
                    string responseBodySkill = await responseSkill.Content.ReadAsStringAsync();
                    var optionSkill = new JsonSerializerOptions()
                    { PropertyNameCaseInsensitive = true };
                    SkillDto = JsonSerializer.Deserialize<SkillDTO>(responseBodySkill, optionSkill);
                }

                var jsonRequestBody = JsonSerializer.Serialize(updatedSkill, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                // Assuming your API endpoint for updating a skill is using the PUT method
                var response = await client.PutAsync(ServiceMangaUrl + $"api/Skill/UpdateSkill/{updatedSkill.SkillID}", content);
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
            catch (Exception ex)
            {
                // Log the exception
                // Example: logger.LogError(ex, "An error occurred during skill update.");

                // You might want to show an error message to the user or handle it in some way
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
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
