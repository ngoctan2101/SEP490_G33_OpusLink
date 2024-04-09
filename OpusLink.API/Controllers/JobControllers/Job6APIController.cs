using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    [Authorize(Roles = "Freelancer")]
    [Route("api/[controller]")]
    [ApiController]
    public class Job6APIController : ControllerBase
    {
        private readonly ISaveJobService saveJobService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;
        public Job6APIController(ISaveJobService saveJobService, IMapper mapper, ICategoryService categoryService)
        {
            this.saveJobService = saveJobService;
            _mapper = mapper;
            this.categoryService = categoryService;
        }
        [HttpPost("GetAllJobSaved/{userId}")]
        public async Task<IActionResult> GetAllJobSaved([FromBody] Filter filter, [FromRoute] int userId)
        {
            int numberOfPage;
            var saveJobs = await saveJobService.GetAllJobSave(userId);
            var saveJobsResultAfterSearch = Search(saveJobs, filter, out numberOfPage);
            List<GetSaveJobResponse> result = _mapper.Map<List<GetSaveJobResponse>>(saveJobsResultAfterSearch);
            for (int i = 0; i < result.Count; i++)
            {
                GetJobResponse gjr = _mapper.Map<GetJobResponse>(saveJobsResultAfterSearch[i].Job);
                result[i].GetJobResponse = gjr;
            }
            result.Add(new GetSaveJobResponse() { SaveJobID = numberOfPage });
            return Ok(result);
        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await categoryService.GetAllCategory();
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            return Ok(result);
        }
        private List<SaveJob> Search(List<SaveJob> saveJobs, Filter filter, out int numberOfPage)
        {
            List<SaveJob> result = new List<SaveJob>();
            result = saveJobs.ToList();
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].Job.JobTitle.ToLower().Contains(filter.SearchStr.ToLower()) || result[i].Job.JobContent.ToLower().Contains(filter.SearchStr.ToLower()))
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
