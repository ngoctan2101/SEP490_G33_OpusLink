using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.OfferControllers
{
    [Authorize(Roles = "Freelancer,Employer")]
    [Route("api/[controller]")]
    [ApiController]
    public class Offer3APIController : ControllerBase
    {
        private readonly IOfferService offerService;
        private readonly IMapper _mapper;
        public Offer3APIController(IOfferService offerService, IMapper mapper)
        {
            this.offerService = offerService;
            _mapper = mapper;
        }
        [HttpGet("GetAllOfferOfJob/{jobId}")]
        public async Task<IActionResult> GetAllOfferOfJob([FromRoute] int jobId)
        {
            List<Offer> offers = await offerService.GetAllOfferOfJob(jobId);
            List<GetOfferAndFreelancerResponse> result = 
                _mapper.Map<List<GetOfferAndFreelancerResponse>>(offers);
            foreach(var offer in result)
            {
                string FreelancerImagePath = offer.FreelancerProfileImage;
                if (FreelancerImagePath == null || FreelancerImagePath.Length == 0)
                {
                    continue;
                }
                string imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", FreelancerImagePath);
                // Check if the file exists
                if (System.IO.File.Exists(imageFilePath))
                {
                    offer.FreelancerImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
                }
            }
            
            return Ok(result);
        }
        [HttpGet("IsOffered/{jobId}/{userId}")]
        public async Task<IActionResult> IsOffered(int jobId, int userId)
        {
            bool result =  offerService.IsOffered(jobId, userId);
            return Ok(result);
        }
        [HttpGet("GetOffer/{jobId}/{userId}")]
        public async Task<IActionResult> GetOffer(int jobId, int userId)
        {
            var result = await offerService.GetOffer(jobId, userId);
            return Ok(_mapper.Map<GetOfferResponse>(result));
        }
        [HttpPost("CreateOffer")]
        public async Task<IActionResult> CreateOffer([FromBody] CreateUpdateOfferRequest offer)
        {
            Offer o = _mapper.Map<Offer>(offer);
            await offerService.CreateOffer(o);
            return Ok();
        }
        [HttpPut("UpdateOffer")]
        public async Task<IActionResult> UpdateOffer([FromBody] CreateUpdateOfferRequest offer)
        {
            Offer o = _mapper.Map<Offer>(offer);
            await offerService.UpdateOffer(o);
            return Ok();
        }
    }
}
