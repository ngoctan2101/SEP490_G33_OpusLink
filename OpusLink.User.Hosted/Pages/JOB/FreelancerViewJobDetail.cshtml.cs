﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Constants;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpusLink.User.Hosted.Pages.JOB
{
    public class FreelancerViewJobDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        public GetJobDetailResponse job { get; set; }
        public List<GetOfferAndFreelancerResponse> offers { get; set; }
        public bool isOffered { get; set; }
        public GetOfferResponse offerResponse { get; set; }
        public FreelancerViewJobDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            offerResponse = new GetOfferResponse();
        }
        public async Task<IActionResult> OnGetAsync(int JobId)
        {

            int userId = 0;
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            
            //HttpContext.Session.SetInt32("UserId", 201); 
            //int userId = 201;

            HttpResponseMessage response = await client.GetAsync(UrlConstant.ApiBaseUrl+"/Job5API/GetJobDetail/" + JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                job = JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
                HttpContext.Session.SetInt32("JobId", JobId);
            }
            else
            {
                HttpResponseMessage responseOther = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Job5API/GetJobDetail/" + HttpContext.Session.GetInt32("JobId"));
                if (responseOther.IsSuccessStatusCode)
                {
                    string strData = await responseOther.Content.ReadAsStringAsync();
                    job = JsonConvert.DeserializeObject<GetJobDetailResponse>(strData);
                }
                responseOther = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/GetAllOfferOfJob/" + HttpContext.Session.GetInt32("JobId"));
                if (responseOther.IsSuccessStatusCode)
                {
                    string strData = await responseOther.Content.ReadAsStringAsync();
                    offers = JsonConvert.DeserializeObject<List<GetOfferAndFreelancerResponse>>(strData);
                    offers = offers.OrderByDescending(o => o.DateOffer).ToList();
                }
                isOffered = false;
                //is offered or not
                responseOther = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/IsOffered/" + JobId + "/" + userId);
                if (responseOther.IsSuccessStatusCode)
                {
                    string strData = await responseOther.Content.ReadAsStringAsync();
                    if (strData.Equals("true"))
                    {
                        isOffered = true;
                        responseOther = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/GetOffer/" + JobId + "/" + userId);
                        if (responseOther.IsSuccessStatusCode)
                        {
                            strData = await responseOther.Content.ReadAsStringAsync();
                            offerResponse = JsonConvert.DeserializeObject<GetOfferResponse>(strData);
                        }
                    }
                    else if (strData.Equals("false"))
                    {
                        isOffered = false;
                    }
                }
                HttpContext.Session.SetString("Notification", "Không tồn tại");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
                return RedirectToPage("/JOB/FreelancerViewJobDetail", new { JobId = HttpContext.Session.GetInt32("JobId") });
            }
            //get list offers for job
            response = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/GetAllOfferOfJob/" + JobId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                offers = JsonConvert.DeserializeObject<List<GetOfferAndFreelancerResponse>>(strData);
                offers = offers.OrderByDescending(o => o.DateOffer).ToList();
            }
            isOffered = false;
            //is offered or not
            response = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/IsOffered/" + JobId + "/" + userId);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                if (strData.Equals("true"))
                {
                    isOffered = true;
                    response = await client.GetAsync(UrlConstant.ApiBaseUrl + "/Offer3API/GetOffer/" + JobId + "/" + userId);
                    if (response.IsSuccessStatusCode)
                    {
                        strData = await response.Content.ReadAsStringAsync();
                        offerResponse = JsonConvert.DeserializeObject<GetOfferResponse>(strData);
                    }
                }
                else if (strData.Equals("false"))
                {
                    isOffered = false;
                }
            }
            return Page();
        }
        public async Task<RedirectToPageResult> OnPostForAddAsync(IFormCollection collection)
        {
            CreateUpdateOfferRequest createUpdateOfferRequest = new CreateUpdateOfferRequest();
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            createUpdateOfferRequest.FreelancerID = HttpContext.Session.GetInt32("UserId") ?? 0;
            
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Offer object
            foreach (string key in keys)
            {
                if (key.Contains("offerId"))
                {
                    createUpdateOfferRequest.OfferID = Int32.Parse(collection[key].ToString().Length==0?"0": (collection[key].ToString()));
                }
                else if (key.Contains("jobId"))
                {
                    createUpdateOfferRequest.JobID= (Int32.Parse(collection[key]));
                }
                else if (key.Contains("dateOffer"))
                {
                    createUpdateOfferRequest.DateOffer = DateTime.Parse( collection[key].ToString().Length==0?DateTime.Now.ToString(): collection[key].ToString());
                }
                else if (key.Contains("proposedCost"))
                {
                    createUpdateOfferRequest.ProposedCost= Decimal.Parse(Regex.Replace(collection[key].ToString(), "[^0-9]", ""));
                }
                else if (key.Contains("expectedDays"))
                {
                    createUpdateOfferRequest.ExpectedDays = Int32.Parse(collection[key].ToString());
                }
                else if (key.Contains("selfIntroduction"))
                {
                    createUpdateOfferRequest.SelfIntroduction=collection[key].ToString();
                }
                else if (key.Contains("estimatedPlan"))
                {
                    createUpdateOfferRequest.EstimatedPlan = collection[key].ToString();
                }
            }
            //post Offer to API 
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateUpdateOfferRequest>(createUpdateOfferRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UrlConstant.ApiBaseUrl+"/Offer3API/CreateOffer", httpContent);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Notification", "Tạo offer thành công");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
            }
            return RedirectToPage("/JOB/FreelancerViewJobDetail", new { JobId = createUpdateOfferRequest.JobID });
        }
        public async Task<RedirectToPageResult> OnPostForUpdateAsync(IFormCollection collection)
        {
            CreateUpdateOfferRequest createUpdateOfferRequest = new CreateUpdateOfferRequest();
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("../Account/Login");
            }
            // Set the JWT token in the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            createUpdateOfferRequest.FreelancerID = HttpContext.Session.GetInt32("UserId") ?? 0;
            
            List<string> keys = collection.Keys.ToList<string>();
            // manual bind to get Offer object
            foreach (string key in keys)
            {
                if (key.Contains("offerId"))
                {
                    createUpdateOfferRequest.OfferID = Int32.Parse(collection[key].ToString().Length == 0 ? "0" : (collection[key].ToString()));
                }
                else if (key.Contains("jobId"))
                {
                    createUpdateOfferRequest.JobID = (Int32.Parse(collection[key]));
                }
                else if (key.Contains("dateOffer"))
                {
                    createUpdateOfferRequest.DateOffer = DateTime.Parse(collection[key].ToString().Length == 0 ? DateTime.Now.ToString() : collection[key].ToString());
                }
                else if (key.Contains("proposedCost"))
                {
                    createUpdateOfferRequest.ProposedCost = Decimal.Parse(Regex.Replace(collection[key].ToString(), "[^0-9]", ""));
                }
                else if (key.Contains("expectedDays"))
                {
                    createUpdateOfferRequest.ExpectedDays = Int32.Parse(collection[key].ToString());
                }
                else if (key.Contains("selfIntroduction"))
                {
                    createUpdateOfferRequest.SelfIntroduction = collection[key].ToString();
                }
                else if (key.Contains("estimatedPlan"))
                {
                    createUpdateOfferRequest.EstimatedPlan = collection[key].ToString();
                }
            }
            //post Offer to API 
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<CreateUpdateOfferRequest>(createUpdateOfferRequest, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UrlConstant.ApiBaseUrl+"/Offer3API/UpdateOffer", httpContent);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Notification", "Sửa offer thành công");
                HttpContext.Session.SetInt32("NotiIsNew", 1);
            }
            return RedirectToPage("/JOB/FreelancerViewJobDetail", new { JobId = createUpdateOfferRequest.JobID });
        }
    }
}
