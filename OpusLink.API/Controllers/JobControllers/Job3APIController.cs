using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.Models.JOB;
using OpusLink.Service.JobServices;

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
        [HttpGet("GetAllJob")]
        public async Task<IActionResult> GetAllJob()
        {
            var jobs = await jobService.GetAllJob(); 
            GetJobResponse[] result = _mapper.Map<GetJobResponse[]>(jobs);
            return Ok(result);
        }
        [HttpGet("GetAllChildCategory")]
        public async Task<IActionResult> GetAllChildCategory(int parentId)
        {
            var categories = await categoryService.GetAllChildCategory(parentId);
            GetCategoryResponse[] result = _mapper.Map<GetCategoryResponse[]>(categories);
            foreach(var category in result)
            {
                if (await categoryService.CountChild(category.CategoryID) > 0)
                {
                    category.HasChildCategory = true;
                }
            }
            return Ok(result);
        }
    }
}
