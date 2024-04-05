using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
using OpusLink.Shared.Enums;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class FreelancerViewAllJobPageModel : PageModel
    {
        private readonly HttpClient client = null;
        public IList<GetJobResponse> Jobs { get; set; } = default!;
        public IList<GetCategoryResponse> Categories { get; set; } = default!;
        public IList<GetCategoryResponse> AllCategories { get; set; } = default!;
        public IList<Int32> AllSavedJobId { get; set; } = default!;
        public Filter filter { get; set; }
        public int NumberOfPage { get; set; }
        public int PageNo { get; set; }
        public bool isCount { get; set; }

        public FreelancerViewAllJobPageModel()
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
                DateMax = DateTime.ParseExact("2024-05-30 23:59", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)

            };
            

        }
        public async Task OnGetAsync()
        {
            if(filter.Statuses.Count == 0) {
                filter.Statuses.Add((int)JobStatusEnum.Hiring);
                //filter.Statuses.Add((int)JobStatusEnum.Hired);
                //filter.Statuses.Add((int)JobStatusEnum.Close);
            }
            //get all first jobs
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Job4API/GetAllJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                Jobs = JsonConvert.DeserializeObject<List<GetJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Jobs.ElementAt(Jobs.Count - 1).NumberOfOffer;
                Jobs.RemoveAt(Jobs.Count - 1);
                Jobs = Jobs.OrderByDescending(j=>j.DateCreated).ToList();
            }
            //get all category has parent is 0
            Categories = await GetListCategoryAsync();
            AllCategories = await GetAllCategoryAsync();
            //get list id of saved jobs
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                AllSavedJobId = await GetListSavedJobId(HttpContext.Session.GetInt32("UserId")??0);
            }
            isCount = false;
            foreach (var j in Jobs) {
                if(j.Status==(int)JobStatusEnum.Hiring && j.EndHiringDate >= DateTime.Now)
                {
                    isCount = true;
                    break;
                }
            }
        }

        private async Task<IList<int>> GetListSavedJobId(int userId)
        {
            
            IList<int> list = new List<int>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            
            string json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job7API/GetAllSavedJobId/" + userId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<int>>(strData);
            }
            return list;
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
                else if (key.Contains("status"))
                {
                    filter.Statuses.Add(Int32.Parse(collection[key].ToString()));
                }
                else if (key.Contains("category"))
                {
                    filter.CategoryIDs.Add(Int32.Parse(collection[key].ToString()));
                }
                else if (key.Contains("price_min"))
                {
                    string price = collection[key].ToString();
                    price = price.Replace(",", string.Empty);
                    price = price.Replace("₫", string.Empty);
                    price = price.Replace(" ", string.Empty);
                    filter.BudgetMin = Int32.Parse(price);
                }
                else if (key.Contains("price_max"))
                {
                    string price = collection[key].ToString();
                    price = price.Replace(",", string.Empty);
                    price = price.Replace("₫", string.Empty);
                    price = price.Replace(" ", string.Empty);
                    filter.BudgetMax = Int32.Parse(price);
                }
                else if (key.Contains("daterange"))
                {
                    string date1 = collection[key].ToString().Split('-')[0].Trim();
                    string date2 = collection[key].ToString().Split('-')[1].Trim();
                    filter.DateMin = DateTime.ParseExact(date1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    filter.DateMax = DateTime.ParseExact(date2, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Job4API/GetAllJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                
                string strData = await response.Content.ReadAsStringAsync();
                Jobs = JsonConvert.DeserializeObject<List<GetJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Jobs.ElementAt(Jobs.Count - 1).NumberOfOffer;
                Jobs.RemoveAt(Jobs.Count - 1);
                Jobs = Jobs.OrderByDescending(j => j.DateCreated).ToList();

            }
            else
            {

            }
            //get all category has parent is 0
            Categories = await GetListCategoryAsync();
            AllCategories = await GetAllCategoryAsync();
            //get list id of saved jobs
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                AllSavedJobId = await GetListSavedJobId(HttpContext.Session.GetInt32("UserId") ?? 0);
            }

        }
        public async Task OnPostForSaveAsync(IFormCollection collection)
        {
            CreateSaveJobRequest saveJobRequest=new CreateSaveJobRequest();
            saveJobRequest.FreelancerID = 111;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Filter object
            foreach (string key in keys)
            {
                if (key.Contains("Search_Str"))
                {
                    filter.SearchStr = collection[key];
                }
                else if (key.Contains("status"))
                {
                    filter.Statuses.Add(Int32.Parse(collection[key].ToString()));
                }
                else if (key.Contains("category"))
                {
                    filter.CategoryIDs.Add(Int32.Parse(collection[key].ToString()));
                }
                else if (key.Contains("price_min"))
                {
                    string price = collection[key].ToString();  
                    price = price.Replace(",", string.Empty);
                    price = price.Replace("₫", string.Empty);
                    price = price.Replace(" ", string.Empty);
                    filter.BudgetMin = Int32.Parse(price);
                }
                else if (key.Contains("price_max"))
                {
                    string price = collection[key].ToString();
                    price = price.Replace(",", string.Empty);
                    price = price.Replace("₫", string.Empty);
                    price = price.Replace(" ", string.Empty);
                    filter.BudgetMax = Int32.Parse(price);
                }
                else if (key.Contains("daterange"))
                {
                    string date1 = collection[key].ToString().Split('-')[0].Trim();
                    string date2 = collection[key].ToString().Split('-')[1].Trim();
                    filter.DateMin = DateTime.ParseExact(date1, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    filter.DateMax = DateTime.ParseExact(date2, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else if (key.Contains("pageNo"))
                {
                    filter.PageNumber = Int32.Parse(collection[key]);
                    PageNo = filter.PageNumber;
                }
                else if (key.Contains("saveJobId"))
                {
                    saveJobRequest.JobID = Int32.Parse(collection[key]);
                }
            }
            //post filter to API 
            //get list 10 job base on Filter
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateSaveJobRequest>(saveJobRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl + "/Job7API/AddSaveJob", httpContent);
            //save xong roi thi hien thi het job lai tu dau
             json = System.Text.Json.JsonSerializer.Serialize<Filter>(filter, options);
             httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
             response = await client.PostAsync(UrlConstant.ApiBaseUrl + "/Job4API/GetAllJob", httpContent);

            if (response.IsSuccessStatusCode)
            {
                
                string strData = await response.Content.ReadAsStringAsync();
                Jobs = JsonConvert.DeserializeObject<List<GetJobResponse>>(strData);
                //tsn goi cai nay la bi thuat :>
                NumberOfPage = Jobs.ElementAt(Jobs.Count - 1).NumberOfOffer;
                Jobs.RemoveAt(Jobs.Count - 1);

                Jobs = Jobs.OrderByDescending(j => j.DateCreated).ToList();
            }
            else
            {

            }
            //get all category has parent is 0
            Categories = await GetListCategoryAsync();
            AllCategories = await GetAllCategoryAsync();
            //get list id of saved jobs

        }

        private async Task<IList<GetCategoryResponse>> GetListCategoryAsync()
        {
            //get all category has parent is 0
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Job4API/GetAllChildCategory?parentId=" + 0);
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
        private async Task<IList<GetCategoryResponse>> GetAllCategoryAsync()
        {
            //get all category
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job4API/GetAllCategory");
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
