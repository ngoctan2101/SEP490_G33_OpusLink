using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;
using OpusLink.Service.MSServices;
using System.Diagnostics;

namespace OpusLink.API.Controllers.MSControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMilestonesAPI : ControllerBase
    {
        private readonly IMilestoneService milestoneService;
        private readonly IMapper _mapper;
        public EMilestonesAPI(IMilestoneService milestoneService, IMapper mapper)
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
        [HttpPost("AddMilestone")]
        public async Task<IActionResult> AddMilestone([FromBody] CreateMilestoneRequest milestone)
        {
            Milestone ms = _mapper.Map<Milestone>(milestone);
            await milestoneService.CreateMilestone(ms);
            return Ok();
        }
    }

}
