using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using OpusLink.Shared.Enums;
using System.Diagnostics;
using System.Reflection;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job4APIController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;

        public Job4APIController(IJobService jobService, ICategoryService categoryService, IMapper mapper)
        {
            this.jobService = jobService;
            this.categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            
            List<GetCategoryResponse> result;
            var categories = await categoryService.GetAllCategory();
            result = _mapper.Map<List<GetCategoryResponse>>(categories);
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            return Ok(result);
        }

        [HttpGet("GetAllChildCategory")]
        public async Task<IActionResult> GetAllChildCategory(int parentId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var categories = await categoryService.GetAllChildCategory(parentId);
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            foreach (var category in result)
            {
                if ( categoryService.HasChild(category.CategoryID))
                {
                    category.HasChildCategory = true;
                }
            }
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            return Ok(result);
        }

        [HttpPost("GetAllJob")]
        public async Task<IActionResult> GetAllJob([FromBody] Filter filter)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<GetJobResponse> result;
            int numberOfPage;
            //var jobs = await jobService.GetAllJob();
            var jobsResultAfterSearch = jobService.Search( filter, true,out numberOfPage);
            result = _mapper.Map<List<GetJobResponse>>(jobsResultAfterSearch);
            result.Add(new GetJobResponse() { NumberOfOffer = numberOfPage });

            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            return Ok(result);
        }

        //private List<Job> Search(List<Job> jobs, Filter filter, out int numberOfPage)
        //{

        //    //// Filter out jobs with status NotApprove
        //    //jobs = jobs.Where(x => x.Status != (int)JobStatusEnum.NotApprove).ToList();

        //    //// Apply category filter
        //    //if (filter.CategoryIDs.Count > 0)
        //    //{
        //    //    jobs = jobs.Where(job => job.JobAndCategories
        //    //        .Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))).ToList();
        //    //}

        //    //// Apply status filter
        //    //if (filter.Statuses.Count > 0)
        //    //{
        //    //    jobs = jobs.Where(job => filter.Statuses.Contains(job.Status)).ToList();
        //    //}

        //    //// Apply budget filter
        //    //jobs = jobs.Where(job => job.BudgetTo >= filter.BudgetMin && job.BudgetFrom <= filter.BudgetMax).ToList();

        //    //// Apply date filter
        //    //jobs = jobs.Where(job => job.DateCreated >= filter.DateMin && job.DateCreated <= filter.DateMax).ToList();

        //    //// Apply search string filter
        //    //if (!string.IsNullOrEmpty(filter.SearchStr))
        //    //{
        //    //    string searchStrLower = filter.SearchStr.ToLower();
        //    //    jobs = jobs.Where(job => job.JobTitle.ToLower().Contains(searchStrLower) ||
        //    //                              job.JobContent.ToLower().Contains(searchStrLower)).ToList();
        //    //}
        //    //Thread.Sleep(1000);
        //    //// Calculate number of pages
        //    //numberOfPage = (int)Math.Ceiling((double)jobs.Count / 6);


        //    //// Return paginated result
        //    //return jobs.Skip((filter.PageNumber - 1) * 6).Take(6).ToList();
        //    jobs = jobs.Where(x => x.Status != (int)JobStatusEnum.NotApprove).ToList();
        //    List<Job> result = new List<Job>();
        //    //loc theo category
        //    if (filter.CategoryIDs.Count == 0)
        //    {
        //        result = jobs.ToList();
        //    }
        //    else
        //    {
        //        foreach (Job job in jobs)
        //        {
        //            foreach (JobAndCategory jac in job.JobAndCategories)
        //            {
        //                bool nextJob = false;
        //                foreach (int categoryID in filter.CategoryIDs)
        //                {
        //                    if (categoryID == jac.CategoryID)
        //                    {
        //                        result.Add(job); nextJob = true; break;
        //                    }
        //                }
        //                if (nextJob)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    //loc theo status
        //    if (filter.Statuses.Count > 0)
        //    {
        //        for (int i = result.Count - 1; i >= 0; i--)
        //        {
        //            bool isOke = false;
        //            foreach (int status in filter.Statuses)
        //            {
        //                if (result[i].Status == status)
        //                {
        //                    isOke = true; break;
        //                }
        //            }
        //            if (isOke == false)
        //            {
        //                result.RemoveAt(i);
        //            }
        //        }
        //    }
        //    //max job >= min filter && min job <= max filter
        //    for (int i = result.Count - 1; i >= 0; i--)
        //    {
        //        if (result[i].BudgetTo >= filter.BudgetMin && result[i].BudgetFrom <= filter.BudgetMax)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            result.RemoveAt(i);
        //        }
        //    }
        //    //date
        //    for (int i = result.Count - 1; i >= 0; i--)
        //    {
        //        if (result[i].DateCreated >= filter.DateMin && result[i].DateCreated <= filter.DateMax)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            result.RemoveAt(i);
        //        }
        //    }
        //    //search string
        //    if (filter.SearchStr.Length > 0)
        //    {
        //        for (int i = result.Count - 1; i >= 0; i--)
        //        {
        //            if (result[i].JobTitle.ToLower().Contains(filter.SearchStr.ToLower()) || result[i].JobContent.ToLower().Contains(filter.SearchStr.ToLower()))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                result.RemoveAt(i);
        //            }
        //        }
        //    }
        //    //loc theo page
        //    numberOfPage = result.Count / 6;
        //    if (result.Count % 6 > 0)
        //    {
        //        numberOfPage++;
        //    }
        //    return result.Skip((filter.PageNumber - 1) * 6).Take(6).ToList();
        //}
    }
}
