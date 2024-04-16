using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using OpusLink.Service.MSServices;
using OpusLink.Service.NotificationServices;
using System.Diagnostics;

namespace OpusLink.API.Controllers.MSControllers
{
    [Authorize(Roles = "Employer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EMilestonesAPI : ControllerBase
    {
        private readonly IMilestoneService milestoneService;
        private readonly INotificationServices notificationService;
        private readonly IMapper _mapper;
        public EMilestonesAPI(IMilestoneService milestoneService, IMapper mapper, INotificationServices notificationService)
        {
            _mapper = mapper;
            this.milestoneService = milestoneService;
            this.notificationService = notificationService;
        }
        [HttpGet("GetAllMilestone/{jobID}")]
        public async Task<IActionResult> GetAllMilestone([FromRoute] int jobID)
        {
            var milestones = await milestoneService.GetAllMilestoneByJobID(jobID);
            List<GetMilestoneResponse> result = _mapper.Map<List<GetMilestoneResponse>>(milestones);
            return Ok(result);
        }
        [HttpPost("AddMilestone")]
        public async Task<IActionResult> AddMilestone([FromBody] CreateMilestoneRequest milestone)
        {
            Milestone ms = _mapper.Map<Milestone>(milestone);
            await milestoneService.CreateMilestone(ms);
            return Ok();
        }
        [HttpPut("UpdateMilestone")]
        public async Task<IActionResult> UpdateMilestone([FromBody] CreateMilestoneRequest milestone)
        {
            await milestoneService.UpdateMilestone(milestone);
            return Ok();
        }
        [HttpDelete("DeleteMilestone/{milestoneID}")]
        public async Task<IActionResult> UpdateMilestone([FromRoute] int milestoneID)
        {
            await milestoneService.DeleteMilestone(milestoneID);
            return Ok();
        }
        [HttpPut("RequestFreelancerAcceptPlan")]
        public async Task<IActionResult> RequestFreelancerAcceptPlan([FromBody] RequestFreelancerAcceptPlan rq)
        {

            await milestoneService.RequestFreelancerAcceptPlan(rq.JobID, rq.DeadlineAccept);
            
            return Ok();
        }
        [HttpGet("GetThisJob/{jobID}")]
        public async Task<IActionResult> GetThisJob([FromRoute] int jobID)
        {
            Job J = await milestoneService.GetThisJob(jobID);
            GetJobDetailResponse result = _mapper.Map<GetJobDetailResponse>(J);
            return Ok(result);
        }

        [HttpPut("RequestPutMoney")]
        public async Task<IActionResult> RequestPutMoney([FromBody] RequestPutMoney rq)
        {

            bool result = await milestoneService.RequestPutMoney(rq.MilestoneId, rq.JobId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not put money");
            }
        }
        [HttpPut("RequestGetBackMoney")]
        public async Task<IActionResult> RequestGetBackMoney([FromBody] RequestGetBackMoney rq)
        {

            bool result = await milestoneService.RequestGetBackMoney(rq.MilestoneId, rq.JobId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not get back money");
            }
        }

        [HttpPut("RequestChangeStatus")]
        public async Task<IActionResult> RequestChangeStatus([FromBody] RequestChangeStatus rq)
        {

            bool result = await milestoneService.RequestChangeStatus(rq.MilestoneId, rq.JobId, rq.Status);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not get back money");
            }
        }

        [HttpPut("RequestExtendDeadline")]
        public async Task<IActionResult> RequestExtendDeadline([FromBody] RequestExtendDeadline rq)
        {

            bool result = await milestoneService.RequestExtendDeadline(rq.MilestoneId, rq.JobId, rq.NewDeadline);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not set new dateline");
            }
        }

        [HttpPut("RequestFailJob")]
        public async Task<IActionResult> RequestFailJob([FromBody] RequestFailJob rq)
        {

            bool result = await milestoneService.RequestFailJob(rq.MilestoneId, rq.JobId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Can not set job fail");
            }
        }
        [HttpGet("IsAllowToGiveFeedback/{jobId}/{employerId}")]
        public async Task<IActionResult> IsAllowToGiveFeedback([FromRoute] int jobId, [FromRoute] int employerId)
        {

            bool result = await milestoneService.IsAllowToGiveFeedback(jobId, employerId);
            return Ok(result);
        }
    }

}
