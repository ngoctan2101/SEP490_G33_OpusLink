using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using OpusLink.Shared.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpusLink.API.Controllers.JobControllers
{
    //[Authorize(Roles = "Freelancer")]

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
        [HttpPost("GetAllJob")]
        public async Task<IActionResult> GetAllJob([FromBody] Filter filter)
        {
            int numberOfPage;
            //var jobs = await jobService.GetAllJob();
            var jobsResultAfterSearch = jobService.Search(filter, false,out numberOfPage);
            List<GetJobResponse> result = _mapper.Map<List<GetJobResponse>>(jobsResultAfterSearch);
            result.Add(new GetJobResponse() { NumberOfOffer = numberOfPage });
            return Ok(result);
        }
        [HttpGet("GetAllJob2")]
        public ActionResult<IEnumerable<GetJobResponse>> GetAllJob2()
        {
            var job = jobService.GetJob();
            List<GetJobResponse> listjob = _mapper.Map<List<GetJobResponse>>(job)
                .ToList();

            return Ok(listjob);
        }
        

    }
}