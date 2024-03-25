using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO;
using System.Net.Http.Headers;

namespace OpusLink.User.Hosted.Pages.HistoryPayment
{
    public class HistoryPaymentModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<HistoryPaymentDTO> historyPaymentDTOs { get; set; } = default!;

        public HistoryPaymentModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task OnGetHistoryPaymentByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7265/api/HistoryPayment/GetDetailHistoryPaymentByUserId/{id}");

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                historyPaymentDTOs = JsonConvert.DeserializeObject<List<HistoryPaymentDTO>>(strData);
            }
            else
            {

            }
        }
        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/HistoryPayment/GetHistoryPayment");

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                historyPaymentDTOs = JsonConvert.DeserializeObject<List<HistoryPaymentDTO>>(strData);
            }
            else
            {

            }
        }
    }
}
