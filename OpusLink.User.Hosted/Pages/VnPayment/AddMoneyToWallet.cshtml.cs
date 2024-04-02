using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using OpusLink.Service.ValidationServices;
using Newtonsoft.Json;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Security.Principal;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace OpusLink.User.Hosted.Pages.VnPayment
{
    public class AddMoneyToWalletModel : PageModel
    {
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
        public AddMoneyToWalletModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            //_validationService = validateService;
        }

        public async Task<IActionResult> OnGet(int UserId)
        {
            //var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            //var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            //var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;

            //var option = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true,
            //};

            //if (!string.IsNullOrEmpty(json))
            //{
            //    var account = JsonConvert.DeserializeObject<Account>(json);

            //    var response = await _client.GetAsync(ApiUri + $"orderdetail/get-orderdetails-customerid/{account.AccountId}");
            //    var strData = await response.Content.ReadAsStringAsync();
            //    orderDetails = System.Text.Json.JsonSerializer.Deserialize<List<OrderDetail>>(strData, option);

            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetUserById/" + UserId);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }

            //var response2 = await _client.GetAsync(ApiUri + $"payment/get-all-payment");
            //var strData2 = await response2.Content.ReadAsStringAsync();
            //payments = System.Text.Json.JsonSerializer.Deserialize<List<Payment>>(strData2, option);
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(KeyPrice))){
                Price = HttpContext.Session.GetString(KeyPrice);
            }
            else
            {
                Price = string.Empty;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(IFormCollection collection)
        {
            //var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            //var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            //var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;
            //int amount = Convert.ToInt32(HttpContext.Request["amount"]);
            List<string> keys = collection.Keys.ToList<string>();
            double price = 0;
            // manual bind to get Filter object


            foreach (string key in keys)
            {
              
                if (key.Contains("amount"))
                {
                    string price1 = collection[key].ToString();
                    price1 = price1.Replace(".", string.Empty);
                    price1 = price1.Replace("₫", string.Empty);
                    price1 = price1.Replace(" ", string.Empty);
                    price1 = price1.TrimStart();
                    price1 = price1.TrimEnd();
                    price1.Trim();

                    price = double.Parse(price1 +".0");

                }
            }
            HttpContext.Session.SetString(KeyPrice,price.ToString());

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return Redirect("/VnPayment/Payment?opuslink=payment&amount="+price);
           
        }
    }
}
