using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using System.Text.Json;
using OpusLink.Entity.Models;
using Newtonsoft.Json;
using System.Text;
using System.Transactions;



namespace OpusLink.Admin.Hosted.Pages.ManagerWithDrawRequest
{
    public class TransferMoneyModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ServiceMangaUrl = "";
        [BindProperty]
        public WithdrawResponseDTO withdraw { get; set; } = null;

        public TransferMoneyModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ServiceMangaUrl = "https://localhost:7265/";
        }
        public async Task<IActionResult> OnGet(int withdrawId)
        {


            HttpResponseMessage responseWithDraw1 = await client.GetAsync(ServiceMangaUrl + "api/WithDrawRequest/GetAllWithdrawRequestById/" + withdrawId);
            if (responseWithDraw1.IsSuccessStatusCode)
            {
                string responseBodyWithDraw1 = await responseWithDraw1.Content.ReadAsStringAsync();
                var optionUser2 = new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true };
                withdraw = System.Text.Json.JsonSerializer.Deserialize<WithdrawResponseDTO>(responseBodyWithDraw1, optionUser2);
            }

            return Page();
        }
        public async Task<IActionResult> OnPost(IFormCollection collection)
        {
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

                if (key.Contains("withdrawid"))
                {
                    withdrawidres = Convert.ToInt32(collection[key].ToString());
                }
                if (key.Contains("username"))
                {
                    usernameres = collection[key].ToString();
                }
                if (key.Contains("bankacccountinfor"))
                {
                    bankacccountinforres = collection[key].ToString();
                }
                if (key.Contains("bankname"))
                {
                    banknameres = collection[key].ToString();
                }
                if (key.Contains("trancode"))
                {
                    trancoderes = collection[key].ToString();
                }
                if (key.Contains("uid"))
                {
                    uidres = Convert.ToInt32(collection[key].ToString());
                }
                if (key.Contains("amount"))
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


            // waillet - tien
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(uidres);
            var content7 = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"api/User/WithdrawMoney/{price/100}/{uidres}", content7);

            //add to history
            HistoryPaymentDTO his = new HistoryPaymentDTO();
            his.Amount = price / 100;
            his.TransactionDate = DateTime.Now;
            his.TransactionCode = trancoderes;
            his.TransactionType = 2;
            his.UserID = uidres;


            var hisroy = System.Text.Json.JsonSerializer.Serialize(his);
            var content8 = new StringContent(hisroy, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceMangaUrl + $"api/HistoryPayment/AddHistoryPayment", content8);
            int HisPayId = 0;
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                HisPayId = Convert.ToInt32(JsonConvert.DeserializeObject<Int32>(strData));

            }

            // update status with draw
            var jsonProduct1 = System.Text.Json.JsonSerializer.Serialize(withdrawidres);
            var content9 = new StringContent(jsonProduct1, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"api/WithDrawRequest/UpdateHisIdWithdrawRequest/{withdrawidres}/{HisPayId}", content9);

            var jsonProduct2 = System.Text.Json.JsonSerializer.Serialize(withdrawidres);
            var content10 = new StringContent(jsonProduct2, Encoding.UTF8, "application/json");
            await client.PutAsync(ServiceMangaUrl + $"api/WithDrawRequest/UpdateHWithdrawRequestByStatusToSuccessfull/{withdrawidres}", content10);



            return Redirect("/ManagerWithDrawRequest/Views");
        }
    }
}
