using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class Job14APIController : Controller
    {
        private readonly IJobService jobService;
        public Job14APIController(IJobService jobService)
        {
            this.jobService = jobService;
        }
        [HttpPut("ApproveJob")]
        public async Task<IActionResult> ApproveJob([FromBody] ApproveRequest job)
        {
            //:))
            await jobService.ApproveJob(job.JobID);
            return Ok();
        }
    }
}
