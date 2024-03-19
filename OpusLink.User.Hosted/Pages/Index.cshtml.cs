using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using System.Net.Http.Headers;
using System.Text;

namespace OpusLink.User.Hosted.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        private string KeyPrice = "_price";
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        //private IValidationService  _validationService;
        [BindProperty]
        public UserDTO user { get; set; } = null!;

        private string LoginKey = "_login";
        //private string DiscountKey = "_discount";
        private string AddressKey = "_address";
        //private string CartKey = "_cart";
        //private string GuestKey = "_guest";

        public string Price { get; set; }
         

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            //_validationService = validateService;
        }
    

        public async Task<ActionResult> OnGetForTest()
        {
            decimal amount = 1000000;
            int userId = 1;
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(userId);
            var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"api/User/UpdateAmount/{amount}/{userId}", content7);

     

            HistoryPaymentDTO his = new HistoryPaymentDTO();
            his.Amount = Convert.ToDecimal(amount);
            his.TransactionDate = DateTime.Now;
            his.TransactionCode = "11111";
            his.TransactionType = 1;
            his.UserID = userId;

            var hisroy = System.Text.Json.JsonSerializer.Serialize(his);
            var content8 = new StringContent(hisroy, Encoding.UTF8, "application/json");
            await client.PostAsync(ServiceMangaUrl + $"api/HistoryPayment/AddHistoryPayment", content8);
            return Page();
        }
        
    }
}
