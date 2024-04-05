using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using System.Diagnostics;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HireFreelancerForJobController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly IMapper _mapper;
        public HireFreelancerForJobController(IJobService jobService, IMapper mapper)
        {
            this.jobService = jobService;
            _mapper = mapper;
        }

        [HttpPut("HireFreelancerForJob")]
        public async Task<IActionResult> HireFreelancerForJob([FromBody] HireFreelancerForJobRequest request)
        {
            //edit job
            await jobService.HireFreelancerForJob(request.FreelancerId, request.JobId);
            return Ok();
        }

        [HttpPut("CancelHireFreelancerForJob")]
        public async Task<IActionResult> CancelHireFreelancerForJob([FromBody] HireFreelancerForJobRequest request)
        {
            //edit job
            await jobService.CancelHireFreelancerForJob(request.FreelancerId, request.JobId);
            return Ok();
        }
    }

}
