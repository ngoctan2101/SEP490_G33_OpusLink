using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpusLink.Entity.Models;
using OpusLink.Entity.Models.JOB;
using OpusLink.Service.JobServices;
using System.Collections.Generic;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job3APIController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;
        public Job3APIController(IJobService jobService, ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.jobService = jobService;
            _mapper = mapper;
        }
        //[HttpGet("GetAllJob")]
        //public async Task<IActionResult> GetAllJob()
        //{
        //    var jobs = await jobService.GetAllJob(); 
        //    List<GetJobResponse> result = _mapper.Map<List<GetJobResponse>>(jobs);
        //    return Ok(result);
        //}
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await categoryService.GetAllCategory();
            List < GetCategoryResponse> result = _mapper.Map< List<GetCategoryResponse>>(categories);
            foreach (var category in result)
            {
                if (await categoryService.CountChild(category.CategoryID) > 0)
                {
                    category.HasChildCategory = true;
                }
            }
            return Ok(result);
        }
        [HttpGet("GetAllChildCategory")]
        public async Task<IActionResult> GetAllChildCategory(int parentId)
        {
            var categories = await categoryService.GetAllChildCategory(parentId);
            List < GetCategoryResponse> result = _mapper.Map< List<GetCategoryResponse>>(categories);
            foreach(var category in result)
            {
                if (await categoryService.CountChild(category.CategoryID) > 0)
                {
                    category.HasChildCategory = true;
                }
            }
            return Ok(result);
        }
        [HttpPost("GetAllJob")]
        public async Task<IActionResult> GetAllJob([FromBody] Filter filter)
        {
            int numberOfPage;
            var jobs = await jobService.GetAllJob();
            var jobsResultAfterSearch= Search(jobs, filter, out numberOfPage);
            List < GetJobResponse> result = _mapper.Map< List<GetJobResponse>>(jobsResultAfterSearch);
            result.Add(new GetJobResponse() { NumberOfOffer=numberOfPage });
            return Ok(result);
        }
        //[HttpGet("GetAllFirstJob")]
        //public async Task<IActionResult> GetAllFirstJob()
        //{
        //    var jobs = await jobService.GetAllFirstJob();
        //    List < GetJobResponse> result = _mapper.Map< List<GetJobResponse>>(jobs);
        //    return Ok(result);
        //}
        //[HttpGet("GetNumberOfPage")]
        //public async Task<IActionResult> GetNumberOfPage()
        //{
        //    int numberOfPage = await jobService.GetNumberOfPage();
        //    return Ok(numberOfPage);
        //}
        private List<Job> Search(List<Job> jobs, Filter filter,out int numberOfPage)
        {
            List<Job> result = new List<Job>();
            //loc theo category
            if(filter.CategoryIDs.Count== 0) {
                result=jobs;
            }
            else
            {
                foreach (Job job in jobs)
                {
                    foreach (JobAndCategory jac in job.JobAndCategories)
                    {
                        bool nextJob = false;
                        foreach (int categoryID in filter.CategoryIDs)
                        {
                            if (categoryID == jac.CategoryID)
                            {
                                result.Add(job); nextJob = true; break;
                            }
                        }
                        if (nextJob)
                        {
                            break;
                        }
                    }
                }
            }

            //loc theo status
            if (filter.Statuses.Count > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    bool isOke = false;
                    foreach (int status in filter.Statuses)
                    {
                        if (result[i].Status == status)
                        {
                            isOke = true; break;
                        }
                    }
                    if (isOke == false)
                    {
                        result.RemoveAt(i);
                    }
                }
            }
            //max job >= min filter && min job <= max filter
            for (int i = result.Count - 1; i >= 0; i--)
            { 
                if(result[i].BudgetTo >= filter.BudgetMin && result[i].BudgetFrom <= filter.BudgetMax)
                {
                    continue;
                }
                else
                {
                    result.RemoveAt(i);
                }
            }
            //date
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (result[i].DateCreated >= filter.DateMin && result[i].DateCreated <= filter.DateMax)
                {
                    continue;
                }
                else
                {
                    result.RemoveAt(i);
                }
            }
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].JobTitle.Contains(filter.SearchStr) || result[i].JobContent.Contains(filter.SearchStr))
                    {
                        continue;
                    }
                    else
                    {
                        result.RemoveAt(i);
                    }
                }
            }
            //loc theo page
            numberOfPage = result.Count / 10;
            if (result.Count % 10 > 0)
            {
                numberOfPage++;
            }
            return result.Skip((filter.PageNumber-1)*10).Take(10).ToList();
        }
    }
}
