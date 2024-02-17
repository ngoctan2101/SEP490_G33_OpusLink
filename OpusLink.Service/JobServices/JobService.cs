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
    public interface IJobService
    {
        Task<Job[]> GetAllJob();
    }
    public class JobService : IJobService
    {
        private readonly OpusLinkDBContext _dbContext;
        public JobService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Job[]> GetAllJob()
        {
            return await _dbContext.Jobs
                .Include("JobAndCategories")
                .Include("JobAndCategories.Category")
                .Include("Employer")
                .Include("Offers")
                .ToArrayAsync();
        }
    }
}
