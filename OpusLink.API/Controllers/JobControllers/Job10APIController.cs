using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using System.Diagnostics;

namespace OpusLink.API.Controllers.JobControllers
{
    //[Authorize(Roles = "Employer")]
    [Route("api/[controller]")]
    [ApiController]
    public class Job10APIController : ControllerBase
    {
        private readonly ILocationService locationService;
        private readonly ICategoryService categoryService;
        private readonly IJobAndCategoryService jobAndCategoryService;
        private readonly IJobService jobService;
        private readonly IMapper _mapper;
        public Job10APIController(ILocationService locationService, ICategoryService categoryService, IJobService jobService, IMapper mapper, IJobAndCategoryService jobAndCategoryService)
        {
            this.locationService = locationService;
            this.jobService = jobService;
            this.categoryService = categoryService;
            _mapper = mapper;
            this.jobAndCategoryService = jobAndCategoryService;
        }

        [HttpGet("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation()
        {
            var locations = await locationService.GetAllLocation();
            List<GetLocationResponse> result = _mapper.Map<List<GetLocationResponse>>(locations);
            return Ok(result);
        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            List<GetCategoryResponse> result;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var categories = await categoryService.GetAllCategory();
            result = _mapper.Map<List<GetCategoryResponse>>(categories);
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
           
            return Ok(result);
        }
        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest createJobRequest)
        {
            Job j = _mapper.Map<Job>(createJobRequest);
            List<JobAndCategory> jac = new List<JobAndCategory>();
            //add job
            int jobId =await jobService.CreateNewJob(j);
            //add JobAndCategory s
            foreach(int i in createJobRequest.CategoryIds)
            {
                jac.Add(new JobAndCategory { CategoryID = i, JobAndCategoryID=0, JobID= jobId }) ;
            }
            await jobAndCategoryService.AddRangeAsync(jac);
            return Ok();
        }
    }
}
