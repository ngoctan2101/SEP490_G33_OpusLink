using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.NotificationDTO;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
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
        public List<HistoryPaymentDTO> listhis { get; set; } = null;
        [BindProperty]
        public WithdrawResponseDTO withdraw { get; set; } = null;
        [BindProperty]
        public int withdrawId { get; set; }
        
        public string mess = "";


        public ViewsModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = UrlConstant.ApiBaseUrl;


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
            HttpContext.Session.SetString("PageNow", "ManagerWithDrawRequest");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage responseWithDraw = await client.GetAsync(ServiceMangaUrl + "/WithDrawRequest/GetAllWithdrawRequestByStatus/" + 1);
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
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            //UpdateHWithdrawRequestByStatusToFail/{wid}/{reason}
            string resonres = "";
            int wid = 0;
            int uidres = 0;
            
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
                if (key.Contains("rejuserid"))
                {
                    uidres = Convert.ToInt32(collection[key].ToString());
                }
            }

            var jsonRequestBody = System.Text.Json.JsonSerializer.Serialize(wid, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ServiceMangaUrl + $"/WithDrawRequest/UpdateHWithdrawRequestByStatusToFail/{wid}/{resonres}", content);

            

            // add notification reject
            NotificationDTO noti = new NotificationDTO();
            noti.UserID = uidres;
            noti.NotificationContent = resonres;
            noti.IsReaded = false;
            //https://localhost:7131/WithDrawMoney/WithDrawMoneyRequest?UserId=1
            noti.Link = "/WithDrawMoney/WithDrawMoneyRequest?UserId=" + uidres + "";
            noti.NotificationDate = DateTime.Now;

            var notifi = System.Text.Json.JsonSerializer.Serialize(noti);
            var content1 = new StringContent(notifi, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await client.PostAsync(ServiceMangaUrl + $"/Notification/AddNotification", content1);

            HttpContext.Session.SetString("Notification", "Đã từ chối yêu cầu rút tiền");
            HttpContext.Session.SetInt32("NotiIsNew", 1);

            return Redirect("/ManagerWithDrawRequest/Views");


        }
        public async Task<IActionResult> OnPostTranferWithDrawAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            List<string> keys = collection.Keys.ToList<string>();
            int withdrawidres = 0;
            string usernameres = "";
            string banknameres = "";
            string bankacccountinforres = "";
            decimal price = 0;
            string trancoderes = "";
            int uidres = 0;
            foreach (string key in keys)
            {

                if (key.Contains("tranwithdrawid"))
                {
                    withdrawidres = Convert.ToInt32(collection[key].ToString());
                }
                if (key.Contains("tranusername"))
                {
                    usernameres = collection[key].ToString();
                }
                if (key.Contains("tranbankinfor"))
                {
                    bankacccountinforres = collection[key].ToString();
                }
                if (key.Contains("tranbankname"))
                {
                    banknameres = collection[key].ToString();
                }
                if (key.Contains("trancode"))
                {
                    trancoderes = collection[key].ToString();
                }
                if (key.Contains("tranuid"))
                {
                    uidres = Convert.ToInt32(collection[key].ToString());
                }
                if (key.Contains("tranamount"))
                {
                    string price1 = collection[key].ToString();
                    price1 = price1.Replace(".", string.Empty);
                    price1 = price1.Replace("₫", string.Empty);
                    price1 = price1.Replace(" ", string.Empty);
                    price1 = price1.TrimStart();
                    price1 = price1.TrimEnd();
                    price1.Trim();

                    price = decimal.Parse(price1);
                }


            }

            // get all historypayment
            HttpResponseMessage responseUser1 = await client.GetAsync(ServiceMangaUrl + $"/HistoryPayment/GetAllHistoryPayment");
            if (responseUser1.IsSuccessStatusCode)
            {
                string responseBodyUser1 = await responseUser1.Content.ReadAsStringAsync();
                var optionUser1 = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                listhis = System.Text.Json.JsonSerializer.Deserialize<List<HistoryPaymentDTO>>(responseBodyUser1, optionUser1);
            }
            bool flat = true;
            foreach(var item in listhis)
            {
                if(item.TransactionCode == trancoderes)
                {
                    //mess = "Mã giao dịch đã tồn tại";
                    flat = false;
                    break;
                }
            }
            if(flat == false)
            {
                //mess = "Mã giao dịch đã tồn tại";
               
                HttpContext.Session.SetString("Notification", "Mã giao dịch đã tồn tại");
                HttpContext.Session.SetInt32("NotiIsNew", 1);

                return RedirectToPage("/ManagerWithDrawRequest/Views");
                
            }

            // waillet - tien
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(uidres);
            var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"/User/WithdrawMoney/{price}/{uidres}", content7);

            //add to history
            HistoryPaymentDTO his = new HistoryPaymentDTO();
            his.Amount = price;
            his.TransactionDate = DateTime.Now;
            his.TransactionCode = trancoderes;
            his.TransactionType = 2;
            his.UserID = uidres;


            var hisroy = System.Text.Json.JsonSerializer.Serialize(his);
            var content8 = new StringContent(hisroy, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + $"/HistoryPayment/AddHistoryPayment", content8);
            int HisPayId = 0;
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                HisPayId = Convert.ToInt32(JsonConvert.DeserializeObject<Int32>(strData));

            }

            // update status with draw
            var jsonProduct1 = System.Text.Json.JsonSerializer.Serialize(withdrawidres);
            var content9 = new StringContent(jsonProduct1, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"/WithDrawRequest/UpdateHisIdWithdrawRequest/{withdrawidres}/{HisPayId}", content9);

            var jsonProduct2 = System.Text.Json.JsonSerializer.Serialize(withdrawidres);
            var content10 = new StringContent(jsonProduct2, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"/WithDrawRequest/UpdateHWithdrawRequestByStatusToSuccessfull/{withdrawidres}", content10);


            //https://localhost:7131/HistoryPayment/HistoryPaymentDetail?payId=1578
            // add new notification
            NotificationDTO noti = new NotificationDTO();
            noti.UserID = uidres;
            noti.NotificationContent = "Rút tiền thành công";
            noti.IsReaded = false;
            noti.Link = "/HistoryPayment/HistoryPaymentDetail?payId="+HisPayId+"";
            noti.NotificationDate = DateTime.Now;

            var notifi = System.Text.Json.JsonSerializer.Serialize(noti);
            var content1 = new StringContent(notifi, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await client.PostAsync(ServiceMangaUrl + $"/Notification/AddNotification", content1);

            HttpContext.Session.SetString("Notification", "Chuyển tiền thành công");
            HttpContext.Session.SetInt32("NotiIsNew", 1);
            //return Redirect("/ManagerWithDrawRequest/Views");
            return RedirectToPage("/ManagerWithDrawRequest/Views");
        }


    }

}

