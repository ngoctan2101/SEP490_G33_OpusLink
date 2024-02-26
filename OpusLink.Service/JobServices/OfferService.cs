using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
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
        Task CreateOffer(Offer offer);
        Task UpdateOffer(Offer offer);
        Task DeleteOffer(int offerId);
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
                .ToListAsync();
        }

        public async Task UpdateOffer(Offer offer)
        {
            Offer a = _dbContext.Offers.Where(c => c.OfferID == offer.OfferID).FirstOrDefault();
            a = offer;
            _dbContext.Entry(a).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
