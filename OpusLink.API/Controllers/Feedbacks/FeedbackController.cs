using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpusLink.API.Hubs;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.FeedbackDTO;
using OpusLink.Service.Chat;
using OpusLink.Service.Feedbacks;

namespace OpusLink.API.Controllers.Feedbacks
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbackController : ControllerBase
	{
		readonly IFeedbackService _feedbackService;
		readonly IMapper _mapper;
		
		public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
		{
			_feedbackService = feedbackService;
			_mapper = mapper;
		}

		[HttpPost("AddFeedback")]
		public IActionResult AddFeedback([FromBody]CreateFeedbackDTO createFeedbackDTO)
		{
			try
			{
				var createFeedback = _feedbackService.CreateFeedback(createFeedbackDTO);
				return Ok(createFeedback);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
