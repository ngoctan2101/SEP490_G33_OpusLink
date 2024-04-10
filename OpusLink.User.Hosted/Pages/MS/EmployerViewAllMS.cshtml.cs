using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
using OpusLink.Shared.Enums;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.MS
{
    public class EmployerViewAllMSModel : PageModel
    {
        private readonly HttpClient client = null;
        public List<GetMilestoneResponse> milestones { get; set; } = default!;
        public int JobID { get; set; }
        public GetJobDetailResponse job { get; set; }
        public EmployerViewAllMSModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> OnGetAsync(int jobID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            //get all milestones of a job
            milestones = await GetAllMilestonesAsync(jobID);
            //get this job also
            job = await GetThisJob(jobID);
            return Page();
        }
        private async Task<GetJobDetailResponse> GetThisJob(int jobID)
        {
            //get the job
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/EMilestonesAPI/GetThisJob/" + jobID);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
            }
            else
            {
                return null;
            }
        }

        private async Task<List<GetMilestoneResponse>> GetAllMilestonesAsync(int jobID)
        {
            JobID = jobID;
            //get all category
            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/GetAllMilestone/" + jobID);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetMilestoneResponse>>(strData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IActionResult> OnPostForAddAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            CreateMilestoneRequest createMilestoneRequest = new CreateMilestoneRequest();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    createMilestoneRequest.JobID = Int32.Parse(collection[key]);
                }
                if (key.Contains("AddMilestoneContent"))
                {
                    createMilestoneRequest.MilestoneContent = collection[key];
                }
                if (key.Contains("AddDeadline"))
                {
                    createMilestoneRequest.Deadline = DateTime.ParseExact(collection[key], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                }
                if (key.Contains("AddAmountToPay"))
                {
                    string amount = collection[key].ToString();
                    amount = amount.Replace(",", string.Empty);
                    amount = amount.Replace("₫", string.Empty);
                    amount = amount.Replace(" ", string.Empty);
                    createMilestoneRequest.AmountToPay = Decimal.Parse(amount);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateMilestoneRequest>(createMilestoneRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/AddMilestone", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
        }
        public async Task<IActionResult> OnPostForUpdateAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            CreateMilestoneRequest createMilestoneRequest = new CreateMilestoneRequest();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    createMilestoneRequest.JobID = Int32.Parse(collection[key]);
                }
                if (key.Contains("UpdateMilestoneID"))
                {
                    createMilestoneRequest.MilestoneID = Int32.Parse(collection[key]);
                }
                if (key.Contains("UpdateMilestoneContent"))
                {
                    createMilestoneRequest.MilestoneContent = collection[key];
                }
                if (key.Contains("UpdateDeadline"))
                {
                    createMilestoneRequest.Deadline = DateTime.ParseExact(collection[key], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                }
                if (key.Contains("UpdateAmountToPay"))
                {
                    string amount = collection[key].ToString();
                    amount = amount.Replace(",", string.Empty);
                    amount = amount.Replace("₫", string.Empty);
                    amount = amount.Replace(" ", string.Empty);
                    createMilestoneRequest.AmountToPay = Decimal.Parse(amount);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateMilestoneRequest>(createMilestoneRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl+"/EMilestonesAPI/UpdateMilestone", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = createMilestoneRequest.JobID });
        }
        public async Task<IActionResult> OnPostForDeleteAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            int idOfMilestone = 0;
            int idOfJob = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    idOfJob = Int32.Parse(collection[key]);
                }
                if (key.Contains("DeleteMilestoneID"))
                {
                    idOfMilestone = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            HttpResponseMessage response = await client.DeleteAsync(UrlConstant.ApiBaseUrl+"/EMilestonesAPI/DeleteMilestone/" + idOfMilestone);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = idOfJob });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = idOfJob });

        }
        public async Task<IActionResult> OnPostForRequestAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestFreelancerAcceptPlan requestFreelancerAcceptPlan = new RequestFreelancerAcceptPlan();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    requestFreelancerAcceptPlan.JobID = Int32.Parse(collection[key]);
                }

                if (key.Contains("RequestDeadline"))
                {
                    requestFreelancerAcceptPlan.DeadlineAccept = DateTime.ParseExact(collection[key], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                }

            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestFreelancerAcceptPlan>(requestFreelancerAcceptPlan, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestFreelancerAcceptPlan", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestFreelancerAcceptPlan.JobID });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestFreelancerAcceptPlan.JobID });
        }
        public async Task<IActionResult> OnPostForPutMoneyAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestPutMoney requestPutMoney = new RequestPutMoney();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("PutMoneyMilestoneID"))
                {
                    requestPutMoney.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestPutMoney.JobId = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestPutMoney>(requestPutMoney, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestPutMoney", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestPutMoney.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestPutMoney.JobId });
        }
        public async Task<IActionResult> OnPostForGetBackMoneyAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestGetBackMoney requestGetBackMoney = new RequestGetBackMoney();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("GetBackMoneyMilestoneID"))
                {
                    requestGetBackMoney.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestGetBackMoney.JobId = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestGetBackMoney>(requestGetBackMoney, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestGetBackMoney", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestGetBackMoney.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestGetBackMoney.JobId });
        }
        public async Task<IActionResult> OnPostForReviewCompleteMilestoneAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestChangeStatus requestChangeStatus = new RequestChangeStatus();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("ReviewMilestoneID"))
                {
                    requestChangeStatus.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestChangeStatus.JobId = Int32.Parse(collection[key]);
                }
            }
            requestChangeStatus.Status = (int)MilestoneStatusEnum.Completed;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestChangeStatus>(requestChangeStatus, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestChangeStatus", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
        }
        public async Task<IActionResult> OnPostForReviewRejectMilestoneAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestChangeStatus requestChangeStatus = new RequestChangeStatus();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("ReviewMilestoneID"))
                {
                    requestChangeStatus.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestChangeStatus.JobId = Int32.Parse(collection[key]);
                }
            }
            requestChangeStatus.Status = (int)MilestoneStatusEnum.EmployerRejected;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestChangeStatus>(requestChangeStatus, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestChangeStatus", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
        }
        public async Task<IActionResult> OnPostForExtendAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestExtendDeadline requestExtendDeadline = new RequestExtendDeadline();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("ExtendDeadlineMilestoneID"))
                {
                    requestExtendDeadline.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("ExtendDeadlineTime"))
                {
                    requestExtendDeadline.NewDeadline = DateTime.ParseExact(collection[key], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                }
                if (key.Contains("JobID"))
                {
                    requestExtendDeadline.JobId = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestExtendDeadline>(requestExtendDeadline, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestExtendDeadline", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestExtendDeadline.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestExtendDeadline.JobId });
        }
        public async Task<IActionResult> OnPostForFailMsAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestChangeStatus requestChangeStatus = new RequestChangeStatus();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("FailMsMilestoneID"))
                {
                    requestChangeStatus.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestChangeStatus.JobId = Int32.Parse(collection[key]);
                }
            }
            requestChangeStatus.Status = (int)MilestoneStatusEnum.Failed;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestChangeStatus>(requestChangeStatus, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestChangeStatus", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
        }
        public async Task<IActionResult> OnPostForFailJobAsync(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestFailJob requestFailJob = new RequestFailJob();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("FailMsMilestoneID"))
                {
                    requestFailJob.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestFailJob.JobId = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestFailJob>(requestFailJob, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestFailJob", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestFailJob.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestFailJob.JobId });
        }

        public async Task<IActionResult> OnPostForAcceptMilestone2Async(IFormCollection collection)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            RequestChangeStatus requestChangeStatus = new RequestChangeStatus();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("AcceptMs2"))
                {
                    requestChangeStatus.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestChangeStatus.JobId = Int32.Parse(collection[key]);
                }
            }
            requestChangeStatus.Status = (int)MilestoneStatusEnum.Completed;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestChangeStatus>(requestChangeStatus, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl + "/EMilestonesAPI/RequestChangeStatus", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
            }
            return RedirectToPage("/MS/EmployerViewAllMS", new { jobID = requestChangeStatus.JobId });
        }
    }
}
