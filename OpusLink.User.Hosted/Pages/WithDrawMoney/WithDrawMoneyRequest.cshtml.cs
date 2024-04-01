using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Shared.VnPay;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using OpusLink.Entity.Models;
using System.Transactions;
using OpusLink.Entity.DTO.WithdrawRequestDTO;

namespace OpusLink.User.Hosted.Pages.WithDrawMoney
{
    public class WithDrawMoneyRequestModel : PageModel
    {

       
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
     
        [BindProperty]
        public UserDTO user { get; set; } = null!;
        public string ErrorKey = "_error";

        public WithDrawMoneyRequestModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
            //_httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> OnGet(int UserId )
        {
            

            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetUserById/" + UserId);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }

           
           
            return Page();
        }

        public async Task<IActionResult> OnPost(IFormCollection collection)
        {
            //var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            //var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            //var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;
            //int amount = Convert.ToInt32(HttpContext.Request["amount"]);

            //if (HttpContext.Session.GetInt32("UserId") == null)
            //{
            //    return RedirectToPage("/Account/Login");
            //}
            //else
            //{
            //    user.Id = HttpContext.Session.GetInt32("UserId") ?? 0;
            //}

            List<string> keys = collection.Keys.ToList<string>();
            double price = 0;
            string bankInfor ="";
            string bankName ="";
            // manual bind to get Filter object
            int userid = 0;

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

                    price = double.Parse(price1 + ".0");
                }
                if (key.Contains("bankacccountinfor"))
                {
                    bankInfor = (collection[key].ToString());
                }
                if (key.Contains("bankacccountinfor"))
                {
                    bankName = collection[key].ToString();
                }
                if (key.Contains("userid"))
                {
                    userid = Convert.ToInt32(collection[key].ToString());
                }
            }
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "api/User/GetUserById/" + userid);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }
            if (user.AmountMoney < Convert.ToDecimal(price))
            {
                HttpContext.Session.SetString(ErrorKey, "Số tiền rút không được quá số dư tài khoản");
                return Page();
            }


            WithdrawRequestDTO wdr = new WithdrawRequestDTO();
            wdr.UserID = userid;
            wdr.Amount = Convert.ToDecimal(price);
            wdr.DateCreated = DateTime.Now;
            wdr.Status = 1;

            var withdraw = System.Text.Json.JsonSerializer.Serialize(wdr);
            var content8 = new StringContent(withdraw, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + $"api/WithDrawRequest/AddWithdrawRequest", content8);
            


            //return RedirectToPage("/HistoryPayment/HistoryPaymentDetail", new { payId = HisPayId });


            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return Redirect("../Index");

        }
    }
}
