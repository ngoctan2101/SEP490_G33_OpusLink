using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job7APIController : Controller
    {
        private readonly ISaveJobService saveJobService;
        private readonly IMapper _mapper;
        public Job7APIController(ISaveJobService saveJobService, IMapper mapper)
        {
            this.saveJobService = saveJobService;
            _mapper = mapper;
        }
        [HttpPost("AddSaveJob")]
        public async Task<IActionResult> AddSaveJob([FromBody] CreateSaveJobRequest saveJob)
        {
            SaveJob saveJob1 = _mapper.Map<SaveJob>(saveJob);
            await saveJobService.CreateSaveJob(saveJob1);
            return Ok();
        }
        [HttpGet("GetAllSavedJobId/{userId}")]
        public async Task<IActionResult> GetAllSavedJobId(int userId)
        {
            List<int> result=await saveJobService.GetAllSavedJobId(userId);
            return Ok(result);
        }
        [HttpDelete("DeleteSaveJob/{saveJobId}")]
        public async Task<IActionResult> DeleteSaveJob(int saveJobId)
        {
            await saveJobService.DeleteSaveJob(saveJobId);
            return Ok();
        }
    }
}
