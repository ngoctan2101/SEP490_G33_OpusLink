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
    public class Job12APIController : Controller
    {
        private readonly IJobService jobService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;

        public Job12APIController(IJobService jobService, ICategoryService categoryService, IMapper mapper)
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
            return Ok(result);
        }
        [HttpGet("GetAllChildCategory")]
        public async Task<IActionResult> GetAllChildCategory(int parentId)
        {
            var categories = await categoryService.GetAllChildCategory(parentId);
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            foreach (var category in result)
            {
                if (categoryService.HasChild(category.CategoryID))
                {
                    category.HasChildCategory = true;
                }
            }
            return Ok(result);
        }
        [HttpPost("GetAllJobRequested")]
        public async Task<IActionResult> GetAllJobRequested([FromBody] Filter filter)
        {
            int numberOfPage;
            var jobsResultAfterSearch = jobService.GetAllJobRequested(filter, out numberOfPage);
            List<GetJobResponse> result = _mapper.Map<List<GetJobResponse>>(jobsResultAfterSearch);
            result.Add(new GetJobResponse() { NumberOfOffer = numberOfPage });
            return Ok(result);
        }

        
    }
}
