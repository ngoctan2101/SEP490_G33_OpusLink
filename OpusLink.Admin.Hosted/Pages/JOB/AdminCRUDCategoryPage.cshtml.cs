using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.Admin.Hosted.Pages.JOB
{
    public class AdminCRUDCategoryPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<GetCategoryResponse> Categories { get; set; } = default!;
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public AdminCRUDCategoryPageModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PageNo = 1;
            //category thi chi can moi search thoi
            filter = new Filter()
            {
                SearchStr = "",
                BudgetMin = 100000,
                BudgetMax = 500000000,
                DateMin = DateTime.ParseExact("2023-01-01 00:01", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateMax = DateTime.ParseExact("2024-03-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };
        }
        public async Task OnGetAsync()
        {
            //get all first categories
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job11API/GetAllCategory", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Int32.Parse(Categories.ElementAt(Categories.Count - 1).CategoryName);
                Categories.RemoveAt(Categories.Count - 1);
            }
            //get all category for "name instead id"
            AllCategories = await GetAllCategoryAsync();
        }

        public async Task OnPostForSearchAsync(IFormCollection collection)
        {
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                //else if (key.Contains("status"))
                //{
                //    filter.Statuses.Add(Int32.Parse(collection[key].ToString()));
                //}
                //else if (key.Contains("category"))
                //{
                //    filter.CategoryIDs.Add(Int32.Parse(collection[key].ToString()));
                //}
                //else if (key.Contains("price_min"))
                //{
                //    string price = collection[key].ToString();
                //    price = price.Replace(",", string.Empty);
                //    price = price.Replace("đ", string.Empty);
                //    price = price.Replace(" ", string.Empty);
                //    filter.BudgetMin = Int32.Parse(price);
                //}
                //else if (key.Contains("price_max"))
                //{
                //    string price = collection[key].ToString();
                //    price = price.Replace(",", string.Empty);
                //    price = price.Replace("đ", string.Empty);
                //    price = price.Replace(" ", string.Empty);
                //    filter.BudgetMax = Int32.Parse(price);
                //}
                //else if (key.Contains("daterange"))
                //{
                //    string date1 = collection[key].ToString().Split('-')[0].Trim();
                //    string date2 = collection[key].ToString().Split('-')[1].Trim();
                //    filter.DateMin = DateTime.ParseExact(date1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //    filter.DateMax = DateTime.ParseExact(date2, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //}
                else if (key.Contains("pageNo"))
                {
                    filter.PageNumber = Int32.Parse(collection[key]);
                    PageNo = filter.PageNumber;
                }
            }
            //post filter to API 
            //get list 10 category base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job11API/GetAllCategory", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Int32.Parse(Categories.ElementAt(Categories.Count - 1).CategoryName);
                Categories.RemoveAt(Categories.Count - 1);
            }
            else
            {

            }
            AllCategories = await GetAllCategoryAsync();
        }
        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/Job11API/GetAllCategory");
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

        public async Task OnPostForAddAsync(IFormCollection collection)
        {
            CreateCategoryRequest category = new CreateCategoryRequest();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("AddCategoryName"))
                {
                    category.CategoryName = collection[key];
                }
                if (key.Contains("AddCategoryParent"))
                {
                    category.ParentID = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateCategoryRequest>(category, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7265/api/Job11API/AddCategory", httpContent);
            
             json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
             httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
             response = await client.PostAsync("https://localhost:7265/api/Job11API/GetAllCategory", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Int32.Parse(Categories.ElementAt(Categories.Count - 1).CategoryName);
                Categories.RemoveAt(Categories.Count - 1);
            }
            //get all category for "name instead id"
            AllCategories = await GetAllCategoryAsync();
        }
        public async Task OnPostForUpdateAsync(IFormCollection collection)
        {
            PutCategoryRequest category = new PutCategoryRequest();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("UpdateCategoryID"))
                {
                    category.CategoryId = Int32.Parse(collection[key]);
                }
                if (key.Contains("UpdateCategoryName"))
                {
                    category.CategoryName = collection[key];
                }
                if (key.Contains("UpdateCategoryParent"))
                {
                    category.ParentID = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<PutCategoryRequest>(category, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7265/api/Job11API/UpdateCategory", httpContent);

            json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync("https://localhost:7265/api/Job11API/GetAllCategory", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Int32.Parse(Categories.ElementAt(Categories.Count - 1).CategoryName);
                Categories.RemoveAt(Categories.Count - 1);
            }
            //get all category for "name instead id"
            AllCategories = await GetAllCategoryAsync();
        }
        public async Task OnPostForDeleteAsync(IFormCollection collection)
        {
            int idOfCategory = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("DeleteCategoryID"))
                {
                    idOfCategory = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            HttpResponseMessage response = await client.DeleteAsync("https://localhost:7265/api/Job11API/DeleteCategory/"+ idOfCategory);
            
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            response = await client.PostAsync("https://localhost:7265/api/Job11API/GetAllCategory", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Categories = JsonConvert.DeserializeObject<List<GetCategoryResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Int32.Parse(Categories.ElementAt(Categories.Count - 1).CategoryName);
                Categories.RemoveAt(Categories.Count - 1);
            }
            //get all category for "name instead id"
            AllCategories = await GetAllCategoryAsync();
        }
    }
}
