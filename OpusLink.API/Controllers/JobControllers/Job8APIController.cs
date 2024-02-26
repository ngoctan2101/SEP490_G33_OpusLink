using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using OpusLink.Shared.Enums;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job8APIController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;

        public Job8APIController(IJobService jobService, ICategoryService categoryService, IMapper mapper)
        {
            this.jobService = jobService;
            this.categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await categoryService.GetAllCategory();
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
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
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            foreach (var category in result)
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
            var jobsResultAfterSearch = Search(jobs, filter, out numberOfPage);
            List<GetJobResponse> result = _mapper.Map<List<GetJobResponse>>(jobsResultAfterSearch);
            result.Add(new GetJobResponse() { NumberOfOffer = numberOfPage });
            return Ok(result);
        }

        private List<Job> Search(List<Job> jobs, Filter filter, out int numberOfPage)
        {
            jobs= jobs.Where(x => x.EmployerID == filter.UserId).ToList();
            List<Job> result = new List<Job>();
            //loc theo category
            if (filter.CategoryIDs.Count == 0)
            {
                result = jobs.ToList();
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
                if (result[i].BudgetTo >= filter.BudgetMin && result[i].BudgetFrom <= filter.BudgetMax)
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
                    if (result[i].JobTitle.ToLower().Contains(filter.SearchStr.ToLower()) || result[i].JobContent.ToLower().Contains(filter.SearchStr.ToLower()))
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
            return result.Skip((filter.PageNumber - 1) * 10).Take(10).ToList();
        }
    }
}
