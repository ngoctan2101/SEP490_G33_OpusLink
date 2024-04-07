﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    //[Authorize(Roles = "Freelancer")]
    [Route("api/[controller]")]
    [ApiController]
    public class Job5APIController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly IMapper _mapper;
        public Job5APIController(IJobService jobService, IMapper mapper)
        {
            this.jobService = jobService;
            _mapper = mapper;
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
    }
}
