﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpusLink.User.Hosted.Pages.MS
{
    public class FreelancerViewAllMSModel : PageModel
    {
        private readonly HttpClient client = null;
        public List<GetMilestoneResponse> milestones { get; set; } = default!;
        public GetJobDetailResponse job { get; set; }
        public int JobID { get; set; }
        public FreelancerViewAllMSModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task OnGetAsync(int jobID)
        {
            //get all milestones of a job
            milestones = await GetAllMilestonesAsync(jobID);
            //get this job also
            job = await GetThisJob(jobID);
        }

        private async Task<GetJobDetailResponse> GetThisJob(int jobID)
        {
            //get the job
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/FMilestonesAPI/GetThisJob/" + jobID);
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
            HttpResponseMessage response = await client.GetAsync("https://localhost:7265/api/FMilestonesAPI/GetAllMilestone/" + jobID);
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

        public async Task<IActionResult> OnPostForNotAcceptPlanAsync(IFormCollection collection)
        {
            int jobID = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    jobID = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<AcceptPlanOrNot>(new AcceptPlanOrNot() { JobID = jobID, Accepted = false }, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7265/api/FMilestonesAPI/AcceptPlanOrNot", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = jobID });
            }
            return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = jobID });
        }
        public async Task<IActionResult> OnPostForAcceptPlanAsync(IFormCollection collection)
        {
            int jobID = 0;
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("JobID"))
                {
                    jobID = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<AcceptPlanOrNot>(new AcceptPlanOrNot() { JobID = jobID, Accepted = true }, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7265/api/FMilestonesAPI/AcceptPlanOrNot", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = jobID });
            }
            return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = jobID });
        }
        public async Task<IActionResult> OnPostForConfirmDoneMilestoneAsync(IFormCollection collection)
        {
            RequestDoneAMilestone requestDoneAMilestone = new RequestDoneAMilestone();
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get  object
            foreach (string key in keys)
            {
                if (key.Contains("ConfirmDoneMilestoneID"))
                {
                    requestDoneAMilestone.MilestoneId = Int32.Parse(collection[key]);
                }
                if (key.Contains("JobID"))
                {
                    requestDoneAMilestone.JobId = Int32.Parse(collection[key]);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<RequestDoneAMilestone>(requestDoneAMilestone, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("https://localhost:7265/api/FMilestonesAPI/DoneAMilestone", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = requestDoneAMilestone.JobId });
            }
            return RedirectToPage("/MS/FreelancerViewAllMS", new { jobID = requestDoneAMilestone.JobId });
        }
    }
}