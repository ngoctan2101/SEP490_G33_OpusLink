using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.JobServices
{
    public interface IOfferService
    {
        Task<List<Offer>> GetAllOffer(int userId);
        Task<List<Offer>> GetAllOfferOfJob(int jobId);
        Task CreateOffer(Offer offer);
        Task UpdateOffer(Offer offer);
        Task DeleteOffer(int offerId);
        bool IsOffered(int jobId, int userId);
        Task<Offer> GetOffer(int jobId, int userId);
    }
    public class OfferService : IOfferService
    {
        private readonly OpusLinkDBContext _dbContext;
        public OfferService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateOffer(Offer offer)
        {
            _dbContext.Offers.Add(offer);
            var j = _dbContext.Jobs.Where(h=>h.JobID== offer.JobID).ToList();   
            Notification n = new Notification()
            {
                NotificationID = 0,
                UserID = j[0].EmployerID,
                NotificationContent = "Job của bạn có thêm một offer.",
                IsReaded = false,
                Link = "/JOB/EmployerViewJobDetailPage?JobId=" + offer.JobID,
                NotificationDate = DateTime.Now
            };
            _dbContext.Notifications.Add(n);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOffer(int offerId)
        {
            Offer O = await _dbContext.Offers.Where(s => s.OfferID==offerId).FirstAsync();
            _dbContext.Remove(O);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllOffer(int userId)
        {
            return await _dbContext.Offers
                .Include("Job")
                .Include("Job.Employer")
                .Include("Job.JobAndCategories.Category")
                .Include("Job.Offers")
                .Include("Job.Location")
                .Where(s => s.FreelancerID == userId)
                .OrderByDescending(s=>s.DateOffer)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllOfferOfJob(int jobId)
        {
            return await _dbContext.Offers.Where(o => o.JobID == jobId).Include("Freelancer").Include("Freelancer.JobsAsAFreelancer").Include("Freelancer.FreelancerAndSkills.Skill").ToListAsync();
        }

        public async Task<Offer> GetOffer(int jobId, int userId)
        {
            return await _dbContext.Offers.Where(o => o.FreelancerID == userId && o.JobID == jobId).FirstOrDefaultAsync();
        }

        public  bool IsOffered(int jobId, int userId)
        {
            return  _dbContext.Offers.Any(o=>o.FreelancerID==userId&&o.JobID==jobId);
        }

        public async Task UpdateOffer(Offer offer)
        {
            Offer a = _dbContext.Offers.Where(c => c.OfferID == offer.OfferID).FirstOrDefault();
            a.ProposedCost = offer.ProposedCost;
            a.ExpectedDays = offer.ExpectedDays;
            a.SelfIntroduction = offer.SelfIntroduction;
            a.EstimatedPlan = offer.EstimatedPlan;
            _dbContext.Entry(a).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
