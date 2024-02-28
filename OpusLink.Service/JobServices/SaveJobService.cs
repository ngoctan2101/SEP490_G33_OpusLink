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
    public interface ISaveJobService
    {
        Task<List<SaveJob>> GetAllJobSave(int userId);
        Task<List<int>> GetAllSavedJobId(int userId);
        Task CreateSaveJob(SaveJob saveJob);
        Task DeleteSaveJob(int saveJobId);
    }
    public class SaveJobService: ISaveJobService
    {
        private readonly OpusLinkDBContext _dbContext;
        public SaveJobService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateSaveJob(SaveJob saveJob)
        {
            _dbContext.SaveJobs.Add(saveJob);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSaveJob(int saveJobId)
        {
            SaveJob sj =await _dbContext.SaveJobs.Where(s=>s.SaveJobID==saveJobId).FirstAsync();
             _dbContext.Remove(sj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SaveJob>> GetAllJobSave(int userId)
        {
            return await _dbContext.SaveJobs
                .Include("Job")
                .Include("Job.Employer")
                .Include("Job.JobAndCategories.Category")
                .Include("Job.Offers")
                .Include("Job.Location")
                .Where(s=>s.FreelancerID==userId)
                .ToListAsync();
        }

        public async Task<List<int>> GetAllSavedJobId(int userId)
        {
            return await _dbContext.SaveJobs.Where(s => s.FreelancerID == userId).Select(u => u.JobID).Distinct()
                .ToListAsync();
        }
    }
}
