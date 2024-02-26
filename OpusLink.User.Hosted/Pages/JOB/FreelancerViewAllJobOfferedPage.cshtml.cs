using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class FreelancerViewAllJobOfferedModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<GetOfferResponse> Offers { get; set; } = default!;
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public int userID { get; set; }

        public FreelancerViewAllJobOfferedModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PageNo = 1;
            filter = new Filter()
            {
                SearchStr = "",
                BudgetMin = 100000,
                BudgetMax = 500000000,
                DateMin = DateTime.ParseExact("2023-01-01 00:01", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateMax = DateTime.ParseExact("2024-04-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };
        }
        public async Task OnGetAsync()
        {
             userID = 111;
            //Get All Offer by userID
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job16API/GetAllOffer/"+ userID, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Offers = JsonConvert.DeserializeObject<List<GetOfferResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Offers.ElementAt(Offers.Count - 1).OfferID;
                Offers.RemoveAt(Offers.Count - 1);
            }
            Offers= Offers.OrderByDescending(o=>o.DateOffer).ToList();
            //get all category
            AllCategories = await GetAllCategoryAsync();
        }

        public async Task OnPostAsync(IFormCollection collection)
        {
            userID = 111;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                else if (key.Contains("pageNo"))
                {
                    filter.PageNumber = Int32.Parse(collection[key]);
                    PageNo = filter.PageNumber;
                }
            }
            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job16API/GetAllOffer/"+userID, httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Offers = JsonConvert.DeserializeObject<List<GetOfferResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Offers.ElementAt(Offers.Count - 1).OfferID;
                Offers.RemoveAt(Offers.Count - 1);
            }
            Offers = Offers.OrderByDescending(o => o.DateOffer).ToList();
            //get all category
            AllCategories = await GetAllCategoryAsync();
        }
        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job16API/GetAllCategory");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
            }
            else
            {
                return null;
            }
        }
    }
}
