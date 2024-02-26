using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
using OpusLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.JobServices
{
    public interface IJobService
    {
        Task<List<Job>> GetAllJobRequested();
        Task<List<Job>> GetAllJob();
        Task ApproveJob(int jobId);
        Task<Job> GetJobDetail(int jobId);
        Task<int> CreateNewJob(Job j);
        Task UpdateOnlyJobProperties(Job a);
    }
    public class JobService : IJobService
    {
        private readonly OpusLinkDBContext _dbContext;
        public JobService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Job>> GetAllJob()
        {
            return await _dbContext.Jobs
                .Include("JobAndCategories")
                .Include("JobAndCategories.Category")
                .Include("Employer")
                .Include("Offers")
                .Include("Location")
                .ToListAsync();
        }
        public async Task<List<Job>> GetAllJobRequested()
        {
            return await _dbContext.Jobs
                .Include("Employer")
                .Where(j=>j.Status==(int)JobStatusEnum.NotApprove)
                .ToListAsync();
        }
        public async Task ApproveJob(int jobId)
        {
            Job a = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstAsync();
            a.Status = (int)JobStatusEnum.Approved;
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<Job> GetJobDetail(int jobId)
        {
            return await _dbContext.Jobs.Where(j => j.JobID == jobId)
                .Include("JobAndCategories")
                .Include("JobAndCategories.Category")
                .Include("Offers")
                .Include("Location")
                .Include("Employer").FirstAsync();
        }

        public async Task<int> CreateNewJob(Job j)
        {
            await _dbContext.Jobs.AddAsync(j);
            await _dbContext.SaveChangesAsync();
            return j.JobID;
        }

        public async Task UpdateOnlyJobProperties(Job a)
        {
            Job b = await _dbContext.Jobs.Where(b=>b.JobID== a.JobID).FirstAsync();
            b.JobTitle= a.JobTitle;
            b.JobContent= a.JobContent;
            b.LocationID= a.LocationID;
            b.BudgetFrom=a.BudgetFrom;
            b.BudgetTo=a.BudgetTo;
            b.Status=a.Status;
            await _dbContext.SaveChangesAsync();
        }
    }
}

