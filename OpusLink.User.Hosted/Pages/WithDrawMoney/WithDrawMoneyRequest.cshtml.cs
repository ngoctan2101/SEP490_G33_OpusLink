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
using Microsoft.Extensions.Options;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Shared.Constants;

namespace OpusLink.User.Hosted.Pages.WithDrawMoney
{
    public class WithDrawMoneyRequestModel : PageModel
    {

       
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
     
        [BindProperty]
        public UserDTO user { get; set; } = null!;
        public string ErrorKey = "";
        public string money = "";

        public WithDrawMoneyRequestModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            //_httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> OnGet(int UserId )
        {
            

            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetUserById/" + UserId);
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
                if (key.Contains("bankname"))
                {
                    bankName = collection[key].ToString();
                }
                if (key.Contains("userid"))
                {
                    userid = Convert.ToInt32(collection[key].ToString());
                }
            }
            HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetUserById/" + userid);
            if (responseUser.IsSuccessStatusCode)
            {
                string responseBodyUser = await responseUser.Content.ReadAsStringAsync();
                var optionUser = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                user = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(responseBodyUser, optionUser);
            }
            if (bankInfor == null)
            {
                ErrorKey = "Thông tin tài khoản không được để trống";
                //HttpContext.Session.SetString(ErrorKey, "Số tiền rút không được quá số dư tài khoản");
                return Page();
            }
            if (bankName == null)
            {
                ErrorKey = "Tên tài khoản không được để trống";
                //HttpContext.Session.SetString(ErrorKey, "Số tiền rút không được quá số dư tài khoản");
                return Page();
            }
            if (user.AmountMoney < Convert.ToDecimal(price))
            {
                ErrorKey = "Số tiền rút không được quá số dư tài khoản";
                //HttpContext.Session.SetString(ErrorKey, "Số tiền rút không được quá số dư tài khoản");
                return Page();
            }


            WithdrawRequestDTO wdr = new WithdrawRequestDTO();
            wdr.UserID = userid;
            wdr.Amount = Convert.ToDecimal(price);
            wdr.DateCreated = DateTime.Now;
            wdr.Status = 1;
            

            var withdraw = System.Text.Json.JsonSerializer.Serialize(wdr);
            var content8 = new StringContent(withdraw, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + $"/WithDrawRequest/AddWithdrawRequest", content8);

            BankAccDTO us = new BankAccDTO();
            us.UserId = userid;
            us.BankAccountInfor = bankInfor;
            us.BankName = bankName;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };


            string json12 = System.Text.Json.JsonSerializer.Serialize<BankAccDTO>(us, options);
            StringContent httpContent23 = new StringContent(json12, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response12 = await client.PutAsync(ServiceMangaUrl + "/User/UpdateBankAccountUser", httpContent23);
            if (response.IsSuccessStatusCode)
            {
                //message "User Edited" green
            }

            if (response12.IsSuccessStatusCode)
            {
                //message "User Edited" green
            }

            //return RedirectToPage("/HistoryPayment/HistoryPaymentDetail", new { payId = HisPayId });


            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return Redirect("../Index");

        }
    }
}
