﻿using Microsoft.AspNetCore.Mvc;
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
using OpusLink.Shared.Constants;

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
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            //_validationService = validateService;
        }

        public async Task<IActionResult> OnGet(int UserId)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            UserId = HttpContext.Session.GetInt32("UserId")??0;

             HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + "/User/GetUserById/" + UserId);
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
