using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.ManagerWithDrawRequest
{
    public class ViewsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";

        [BindProperty]
        public UserDTO user { get; set; } = null!;
        [BindProperty]
        public List<WithdrawResponseDTO> listWithdraw { get; set; } = null;
        [BindProperty]
        public WithdrawResponseDTO withdraw { get; set; } = null;
        [BindProperty]
        public int withdrawId { get; set; }


        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
           

        }
        //public async void OnGetAsync(int WithdrawRequestID)
        //{
        //    HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"api/WithDrawRequest/GetAllWithdrawRequestById/{WithdrawRequestID}");
        //    if (responseUser.IsSuccessStatusCode)
        //    {
        //        string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
        //        var optionUser = new JsonSerializerOptions()
        //        { PropertyNameCaseInsensitive = true };
        //        withdraw = JsonSerializer.Deserialize<WithdrawResponseDTO>(responseBodyUser, optionUser);
        //    }
        //}

        public async Task<IActionResult> OnGet()
        {

            HttpResponseMessage responseWithDraw = await client.GetAsync(ServiceMangaUrl + "api/WithDrawRequest/GetAllWithdrawRequestByStatus/" + 1);
            if (responseWithDraw.IsSuccessStatusCode)
            {
                string responseBodyWithDraw = await responseWithDraw.Content.ReadAsStringAsync();
                var optionUser1 = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listWithdraw = System.Text.Json.JsonSerializer.Deserialize<List<WithdrawResponseDTO>>(responseBodyWithDraw, optionUser1);
            }
            //int withdrawId = HttpContext.Request["withdrawId"];

            //withdrawId = Convert.ToInt32(Request.Form["withdrawId"]);
            //withdrawId = Convert.ToInt32(Request.Query["withdrawId"]);

            //HttpResponseMessage responseWithDraw1 = await client.GetAsync(ServiceMangaUrl + "api/WithDrawRequest/GetAllWithdrawRequestById/" + withdrawId);
            //if (responseWithDraw1.IsSuccessStatusCode)
            //{
            //    string responseBodyWithDraw1 = await responseWithDraw1.Content.ReadAsStringAsync();
            //    var optionUser2 = new JsonSerializerOptions()
            //    { PropertyNameCaseInsensitive = true };
            //    withdraw = System.Text.Json.JsonSerializer.Deserialize<WithdrawResponseDTO>(responseBodyWithDraw1, optionUser2);
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateWithDrawAsync(IFormCollection collection)
        {
            //UpdateHWithdrawRequestByStatusToFail/{wid}/{reason}
            string resonres = "";
            int wid = 0;

            List<string> keys = collection.Keys.ToList<string>();
            foreach (string key in keys)
            {

                if (key.Contains("withdrawid"))
                {
                    wid = Convert.ToInt32(collection[key].ToString());
                }
                if (key.Contains("reason"))
                {
                    resonres = collection[key].ToString();
                }
            }

            var jsonRequestBody = JsonSerializer.Serialize(wid, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ServiceMangaUrl + $"api/WithDrawRequest/UpdateHWithdrawRequestByStatusToFail/{wid}/{resonres}", content);

            return Redirect("/ManagerWithDrawRequest/Views");


        }
    }
}
