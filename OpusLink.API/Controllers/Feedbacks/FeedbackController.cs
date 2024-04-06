using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpusLink.API.Hubs;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.FeedbackDTO;
using OpusLink.Entity.DTO.FeedbackDTOs;
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

        [HttpGet("GetFeedbacksByTargetUser/{targetToUserID}")]
        public IActionResult GetFeedbacksByTargetUser(int targetToUserID)
        {
            try
            {
                var feedbacks = _feedbackService.GetFeedbacksForTargetUser(targetToUserID);
                if (feedbacks == null || feedbacks.Count == 0)
                {
                    return NotFound("No feedbacks found for the specified user.");
                }
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("AverageStars/{targetToUserID}")]
        public IActionResult AverageStars(int targetToUserID)
        {
            try
            {
                // Fetch feedback entries for the given TargetToUserID
                var feedbackEntries = _feedbackService.GetFeedbacksForTargetUser(targetToUserID);

                // Calculate average stars
                var averageStars = feedbackEntries.Any() ? feedbackEntries.Average(f => f.Star) : 0;

                // Count the number of feedback entries
                var numberOfEntries = feedbackEntries.Count;

                // Create an anonymous object to return both averageStars and numberOfEntries
                var result = new
                {
                    AverageStars = averageStars,
                    NumberOfEntries = numberOfEntries
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("UpdateFeedback/{feedbackUserID}")]
        public IActionResult UpdateFeedback(int feedbackUserID, [FromBody]UpdateFeedbackDTO updateFeedbackDTO)
        {
            try
            {
                var updatedFeedback = _feedbackService.UpdateFeedback(feedbackUserID, updateFeedbackDTO.Star, updateFeedbackDTO.Content);
                if (updatedFeedback == null)
                {
                    return NotFound("Feedback not found.");
                }
                return Ok(updatedFeedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
