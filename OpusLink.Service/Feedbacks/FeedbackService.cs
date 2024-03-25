using OpusLink.Entity;
using OpusLink.Entity.DTO.FeedbackDTO;
using OpusLink.Entity.DTO.FeedbackDTOs;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.Feedbacks
{
    public interface IFeedbackService
	{
		FeebackDTO CreateFeedback(CreateFeedbackDTO createFeedbackDTO);
		/*Task<bool> UpdateFeedbackAsync(UpdateFeedbackDTO updateFeedbackDTO);

		Task<FeedbackUser> GetFeedbackAsync(int feedbackUserID);
		Task<IEnumerable<FeedbackUser>> GetFeedbacksAsync(int jobID);*/
	}
	public class FeedbackService : IFeedbackService
	{
		private readonly OpusLinkDBContext _context;

		public FeedbackService(OpusLinkDBContext context)
		{
			_context = context;
		}

		public FeebackDTO CreateFeedback(CreateFeedbackDTO createFeedbackDTO)
		{
				var feedback = new FeedbackUser
				{
					FeedbackUserID = 0,
					JobID = createFeedbackDTO.JobID,
					CreateByUserID = createFeedbackDTO.CreateByUserID,
					TargetToUserID = createFeedbackDTO.TargetToUserID,
					Star = createFeedbackDTO.Star,
					Content = createFeedbackDTO.Content,
					DateCreated = DateTime.Now
				};
			_context.FeedbackUsers.Add(feedback);
			_context.SaveChanges();
			return new FeebackDTO
			{

				FeedbackUserID = feedback.FeedbackUserID

			};
		}

	}
	}

