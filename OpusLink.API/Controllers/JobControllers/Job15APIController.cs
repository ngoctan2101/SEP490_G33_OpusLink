using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
namespace OpusLink.API.Controllers.JobControllers
{
    //[Authorize(Roles = "Admin")]

    [Route("api/[controller]")]
    [ApiController]
    public class Job15APIController : ControllerBase
    {
        private readonly ILocationService locationService;
        private readonly ICategoryService categoryService;
        private readonly IJobAndCategoryService jobAndCategoryService;
        private readonly IJobService jobService;
        private readonly IMapper _mapper;
        public Job15APIController(IJobService jobService, IMapper mapper, ICategoryService categoryService, ILocationService locationService, IJobAndCategoryService jobAndCategoryService)
        {
            this.jobService = jobService;
            _mapper = mapper;
            this.categoryService = categoryService;
            this.locationService = locationService;
            this.jobAndCategoryService = jobAndCategoryService;
        }
        [HttpGet("GetJobDetail/{jobId}")]
        public async Task<IActionResult> GetJobDetail(int jobId)
        {
            var job = await jobService.GetJobDetail(jobId);
            GetJobDetailResponse result = _mapper.Map<GetJobDetailResponse>(job);
            string EmployerImagePath = result.EmployerImagePath;
            if (EmployerImagePath == null || EmployerImagePath.Length == 0)
            {
                return Ok(result);
            }
            string imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", EmployerImagePath);
            // Check if the file exists
            if (System.IO.File.Exists(imageFilePath))
            {
                result.EmployerImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
            }
            return Ok(result);
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
            var categories = await categoryService.GetAllCategory();
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            return Ok(result);
        }
        [HttpPut("EditJob")]
        public async Task<IActionResult> EditJob([FromBody] PutJobRequest putJobRequest)
        {
            Job a = _mapper.Map<Job>(putJobRequest);
            await jobService.UpdateOnlyJobProperties(a);
            List<JobAndCategory> jacs = await jobAndCategoryService.getAllJACOfJob(a.JobID);
            //find list jac to delete
            List<JobAndCategory> jacd = FindJAC2Delete(jacs, putJobRequest.CategoryIDs);
            //find list jac to add
            List<JobAndCategory> jaca = FindJAC2Add(jacs, putJobRequest.CategoryIDs,a.JobID);
            await jobAndCategoryService.DeleteRangeAsync(jacd);
            await jobAndCategoryService.AddRangeAsync(jaca);
            return Ok();
        }

        private List<JobAndCategory> FindJAC2Add(List<JobAndCategory> jacs, List<int> categoryIDs, int jobID)
        {
            List<JobAndCategory> jacResult2 = new List<JobAndCategory>();
            foreach (int jac in categoryIDs)
            {
                if (!(jacs.Select(a=>a.CategoryID).ToList()).Contains(jac))
                {
                    jacResult2.Add(new JobAndCategory { JobAndCategoryID=0,JobID=jobID, CategoryID=jac});
                }
            }
            return jacResult2;
        }

        private List<JobAndCategory> FindJAC2Delete(List<JobAndCategory> jacs, List<int> categoryIDs)
        {
            List<JobAndCategory> jacResult1 = new List<JobAndCategory>();
            foreach(JobAndCategory jac in jacs)
            {
                if (!categoryIDs.Contains(jac.CategoryID))
                {
                    jacResult1.Add(jac);
                }
            }
            return jacResult1;
        }
    }
}
