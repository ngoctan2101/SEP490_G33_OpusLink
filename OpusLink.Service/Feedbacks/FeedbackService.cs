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
		//GetFeedbacksForTargetUser(targetToUserID)
		List<FeebackDTO> GetFeedbacksForTargetUser(int targetToUserID);

        FeebackDTO UpdateFeedback(int feedbackUserID, decimal newStar, string newContent);

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
        //GetFeedbacksForTargetUser(targetToUserID)
        public List<FeebackDTO> GetFeedbacksForTargetUser(int targetToUserID)
        {
            // Fetch feedback entries for the given TargetToUserID
            var feedbackEntries = _context.FeedbackUsers
                .Where(f => f.TargetToUserID == targetToUserID)
                .Select(f => new FeebackDTO
                {
                    FeedbackUserID = f.FeedbackUserID,
                    JobID = f.JobID,
                    CreateByUserID = f.CreateByUserID,
                    TargetToUserID = f.TargetToUserID,
                    Star = f.Star,
                    Content = f.Content,
                    DateCreated = f.DateCreated
					
                })
                .ToList();

            return feedbackEntries;
        }
       public FeebackDTO UpdateFeedback(int feedbackUserID, decimal newStar, string newContent)
		{
            var feedback = _context.FeedbackUsers
                .Where(f => f.FeedbackUserID == feedbackUserID)
                .FirstOrDefault();

            if (feedback == null)
			{
                return null;
            }

            feedback.Star = newStar;
            feedback.Content = newContent;

            _context.SaveChanges();

            return new FeebackDTO
            {
                FeedbackUserID = feedback.FeedbackUserID,
                JobID = feedback.JobID,
                CreateByUserID = feedback.CreateByUserID,
                TargetToUserID = feedback.TargetToUserID,
                Star = feedback.Star,
                Content = feedback.Content,
                DateCreated = feedback.DateCreated
            };
        }
    }
}
