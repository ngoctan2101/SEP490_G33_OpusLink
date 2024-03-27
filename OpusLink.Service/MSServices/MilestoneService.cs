using OpusLink.Entity.Models;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpusLink.Service.MSServices
{
    public interface IMilestoneService
    {
        Task CreateMilestone(Milestone ms);
        Task<List<Milestone>> GetAllMilestoneByJobID(int jobID);
    }
    public class MilestoneService : IMilestoneService
    {
        private readonly OpusLinkDBContext _dbContext;
        public MilestoneService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateMilestone(Milestone ms)
        {
            _dbContext.Milestones.Add(ms);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Milestone>> GetAllMilestoneByJobID(int jobID)
        {
            return await _dbContext.Milestones
                .Where(m=>m.JobID == jobID)
                .ToListAsync();
        }

    }
}
