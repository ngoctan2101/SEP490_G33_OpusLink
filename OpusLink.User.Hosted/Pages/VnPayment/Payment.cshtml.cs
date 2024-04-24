using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using OpusLink.Shared.VnPay;
using System.Globalization;
using System.Security.Principal;
using System.Text.Json;
using System.Text;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;

namespace OpusLink.User.Hosted.Pages.VnPayment
{
    public class PaymentModel : PageModel
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        private string ErrorKey = "_error";
        private string LoginKey = "_login";

       // List<HistoryPaymentDTO> his {  get; set; }

        public PaymentModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;
            //_httpContextAccessor = httpContextAccessor;
           
        }
        public async Task<IActionResult> OnGet()
        {
            double amount = Convert.ToDouble(Request.Query["amount"]);
            int userId = 0;
            //int userId = 0;
            //if (HttpContext.Session.GetInt32("UserId") == null)
            //{
            //    return RedirectToPage("/Login_Register/Login");
            //}
            //else
            //{
            //    userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            //}



            var service = string.Empty;
            var json = HttpContext.Session.GetString(LoginKey) ?? string.Empty;
            //var jsonCart = HttpContext.Session.GetString(CartKey) ?? string.Empty;
            //var jsonGuest = HttpContext.Session.GetString(GuestKey) ?? string.Empty;
            //var jsonCoupon = HttpContext.Session.GetString(DiscountKey) ?? string.Empty;

            string transactionCode = DateTime.Now.Ticks.ToString();
            string amountBack = Convert.ToString(amount * 100);
            OpusLink.Entity.Models.User account = null;

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!string.IsNullOrEmpty(Request.Query["opuslink"]))
            {
                service = Request.Query["opuslink"].ToString();
            }

            if (service.Equals("payment"))
            {
                
                //int userId = 0;
              
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToPage("/Login_Register/Login");
                }
                else
                {
                    userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }

                string url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string returnUrl = UrlConstant.UserClientBaseUrl+"/VnPayment/Payment?opuslink=paymentconfirm";
                
                string tmnCode = "NVFQP1WS";
                string hashSecret = "MPLXERAVPCPUNWPZSLRHRYYBKXYEAVXK";
                PayLib pay = new PayLib();

                //OpusLink.Entity.Models.User us = new Entity.Models.User();
                //double price = Convert.ToDouble(us.AmountMoney) + amount;
                double price = amount;

                var products = string.Empty;
                //user

                if (!string.IsNullOrEmpty(json))
                {
                    account = JsonConvert.DeserializeObject<OpusLink.Entity.Models.User>(json);
                }
               
                //products = "Nap tien";

               
                pay.AddRequestData("vnp_Version", "2.1.0");
                pay.AddRequestData("vnp_Command", "pay");
                pay.AddRequestData("vnp_TmnCode", tmnCode);
                pay.AddRequestData("vnp_Amount", amountBack);
                pay.AddRequestData("vnp_BankCode", "");
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", "VND");
                pay.AddRequestData("vnp_IpAddr", GetIpAddress());
                pay.AddRequestData("vnp_Locale", "vn");
                pay.AddRequestData("vnp_OrderInfo", $"Payment order {"Nap tien"}");
                pay.AddRequestData("vnp_OrderType", "other");
                pay.AddRequestData("vnp_ReturnUrl", returnUrl);
                pay.AddRequestData("vnp_TxnRef", transactionCode);

                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                return Redirect(paymentUrl);
            }

            if (service.Equals("paymentconfirm"))
            {

                //int userId = 0;

                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToPage("/Login_Register/Login");
                }
                else
                {
                    userId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }

                if (Request.Query.Count > 0)
                {
                    string hashSecret = "MPLXERAVPCPUNWPZSLRHRYYBKXYEAVXK";
                    var vnpayData = Request.Query;
                    PayLib pay = new PayLib();

                    foreach (var kvp in vnpayData)
                    {
                        string key = kvp.Key;
                        string value = kvp.Value;

                        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                        {
                            pay.AddResponseData(key, value);
                        }
                    }

                    double amountBack1 = Convert.ToDouble(pay.GetResponseData("vnp_Amount"));
                    long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                    long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                    string vnp_SecureHash = Request.Query["vnp_SecureHash"];

                    bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00")
                        {

                            //HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"api/User/GetUserById/{userId}");
                            //var strData = await responseUser.Content.ReadAsStringAsync();
                            //var us = System.Text.Json.JsonSerializer.Deserialize<OpusLink.Entity.Models.User>(strData, option);

                            //us.AmountMoney =us.AmountMoney + Convert.ToDecimal(amount);
                            // Set the JWT token in the authorization header
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                            // update amount
                            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(userId);
                            var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
                            await client.PutAsync(ServiceMangaUrl + $"/User/UpdateAmount/{amountBack1/100}/{userId}", content7);

                            // add to history
                            HistoryPaymentDTO his = new HistoryPaymentDTO();
                            his.Amount = Convert.ToDecimal(amountBack1/100);
                            his.TransactionDate = DateTime.Now;
                            his.TransactionCode = transactionCode;
                            his.TransactionType = 1;
                            his.UserID = userId;

                            //HistoryPaymentDTO his = new HistoryPaymentDTO();
                            //his.Amount = Convert.ToDecimal(amount);
                            //his.TransactionDate = DateTime.Now;
                            //his.TransactionCode = "11111";
                            //his.TransactionType = 1;
                            //his.UserID = userId;

                            var hisroy = System.Text.Json.JsonSerializer.Serialize(his);
                            var content8 = new StringContent(hisroy, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + $"/HistoryPayment/AddHistoryPayment", content8);
                            int HisPayId = 0;
                            if (response.IsSuccessStatusCode)
                            {
                                string strData = await response.Content.ReadAsStringAsync();
                                HisPayId = Convert.ToInt32(JsonConvert.DeserializeObject<Int32>(strData));
                                
                            }


                            //HttpResponseMessage responseUser = await client.GetAsync(ServiceMangaUrl + $"api/User/GetUserById/{userId}");
                            //var strData = await responseUser.Content.ReadAsStringAsync();
                            //var us = System.Text.Json.JsonSerializer.Deserialize<OpusLink.Entity.Models.User>(strData, option);
                            HttpContext.Session.Remove("PayIdCheck");
                            //HttpContext.Session.SetString(ErrorKey, "");
                            HttpContext.Session.SetString("Notification", "Nạp tiền thành công");
                            HttpContext.Session.SetInt32("NotiIsNew", 1);
                            return RedirectToPage("/HistoryPayment/HistoryPaymentDetail", new { payId = HisPayId });
                           

                        }
                        else
                        {
                            ErrorKey = "Đã xảy ra lỗi khi xử lý hóa đơn | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode + "";
                            //HttpContext.Session.SetString(ErrorKey, "An error occurred while processing the invoice | Trading code: " + vnpayTranId + " | Error code: " + vnp_ResponseCode);
                            return RedirectToPage("/VnPayment/AddMoneyToWallet", new { errorKey = ErrorKey });
                        }
                    }
                    else
                    {
                        ErrorKey = "Một lỗi đã xảy ra trong quá trình xử lý";
                        //HttpContext.Session.SetString(ErrorKey, "An error occurred during processing");
                        return RedirectToPage("/VnPayment/AddMoneyToWallet", new { errorKey = ErrorKey });
                    }
                }

            }
            return Page();

        }

        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"];

                if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
                {
                    ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                }
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP: " + ex.Message;
            }

            return ipAddress;
        }
    }
}
