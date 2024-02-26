using OpusLink.Entity.Models;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpusLink.Service.JobServices
{
    public interface IJobAndCategoryService
    {
        Task AddRangeAsync(List<JobAndCategory> jac);
        Task DeleteJob(int jobId);
        Task DeleteRangeAsync(List<JobAndCategory> jacd);
        Task<List<JobAndCategory>> getAllJACOfJob(int jobID);
    }
    public class JobAndCategoryService : IJobAndCategoryService
    {
        private readonly OpusLinkDBContext _dbContext;
        public JobAndCategoryService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(List<JobAndCategory> jac)
        {
            await _dbContext.AddRangeAsync(jac);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<JobAndCategory> jacd)
        {
            _dbContext.RemoveRange(jacd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<JobAndCategory>> getAllJACOfJob(int jobID)
        {
            return await _dbContext.JobAndCategories.Where(jac=>jac.JobID== jobID).ToListAsync();
        }
        public async Task DeleteJob(int jobId)
        {
            //delete a Job along with associated tables
            using(var _dbContext = new OpusLinkDBContext())
            {
                List<Offer> offers = await _dbContext.Offers.Where(o => o.JobID == jobId).ToListAsync();
                List<JobAndCategory> jacs = await _dbContext.JobAndCategories.Where(jac => jac.JobID == jobId).ToListAsync();
                List<SaveJob> sj = await _dbContext.SaveJobs.Where(s => s.JobID == jobId).ToListAsync();
                List<ReportJob> rj = await _dbContext.ReportJobs.Where(r => r.TargetToJob == jobId).ToListAsync();
                List<Milestone> m = await _dbContext.Milestones.Where(r => r.JobID == jobId).ToListAsync();
                Job jobToDelete = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstOrDefaultAsync();
                if (jobToDelete != null)
                {
                    _dbContext.RemoveRange(offers);
                    _dbContext.RemoveRange(jacs);
                    _dbContext.RemoveRange(sj);
                    _dbContext.RemoveRange(rj);
                    _dbContext.RemoveRange(m);
                    _dbContext.Remove(jobToDelete);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.Dispose();
                }
                else
                {
                    throw new Exception(" null");
                }
            }
            
        }
    }
}
