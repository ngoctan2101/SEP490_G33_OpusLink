using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.OfferControllers
{
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
                offer.FreelancerImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
            }
            
            return Ok(result);
        }
    }
}
