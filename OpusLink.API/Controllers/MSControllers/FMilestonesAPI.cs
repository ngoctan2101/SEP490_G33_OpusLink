using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.MSServices;

namespace OpusLink.API.Controllers.MSControllers
{
    [Authorize(Roles = "Freelancer")]
    [Route("api/[controller]")]
    [ApiController]
    public class fMilestonesAPI : ControllerBase
    {
        private readonly IMilestoneService milestoneService;
        private readonly IMapper _mapper;
        public fMilestonesAPI(IMilestoneService milestoneService, IMapper mapper)
        {
            _mapper = mapper;
            this.milestoneService = milestoneService;
        }
        [HttpGet("GetAllMilestone/{jobID}")]
        public async Task<IActionResult> GetAllMilestone([FromRoute] int jobID)
        {
            var milestones = await milestoneService.GetAllMilestoneByJobID(jobID);
            List<GetMilestoneResponse> result = _mapper.Map<List<GetMilestoneResponse>>(milestones);
            return Ok(result);
        }
        
        [HttpGet("GetThisJob/{jobID}")]
        public async Task<IActionResult> GetThisJob([FromRoute] int jobID)
        {
            Job J = await milestoneService.GetThisJob(jobID);
            GetJobDetailResponse result = _mapper.Map<GetJobDetailResponse>(J);
            return Ok(result);
        }
        [HttpPut("AcceptPlanOrNot")]
        public async Task<IActionResult> AcceptPlanOrNot([FromBody] AcceptPlanOrNot request)
        {
            await milestoneService.AcceptPlanOrNot(request.JobID,request.Accepted);
            return Ok();
        }
        [HttpPut("DoneAMilestone")]
        public async Task<IActionResult> DoneAMilestone([FromBody] RequestDoneAMilestone rq)
        {

            bool result = await milestoneService.RequestDoneAMilestone(rq.MilestoneId, rq.JobId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not set status to review");
            }
        }

    }
}
