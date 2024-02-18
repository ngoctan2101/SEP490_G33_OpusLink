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
        //Task<List<Job>> GetAllFirstJob();
        Task<List<Job>> GetAllJob();
        //Task<int> GetNumberOfPage();
    }
    public class JobService : IJobService
    {
        private readonly OpusLinkDBContext _dbContext;
        public JobService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<List<Job>> GetAllFirstJob()
        //{
        //    return await _dbContext.Jobs
        //        .Include("JobAndCategories")
        //        .Include("JobAndCategories.Category")
        //        .Include("Employer")
        //        .Include("Offers").Take(10)
        //        .ToListAsync();
        //}

        public async Task<List<Job>> GetAllJob()
        {
            return await _dbContext.Jobs
                .Include("JobAndCategories")
                .Include("JobAndCategories.Category")
                .Include("Employer")
                .Include("Offers")
                .ToListAsync();
        }
        //public async Task<int> GetNumberOfPage()
        //{
        //    int count = await _dbContext.Jobs.CountAsync();
        //    int page = count / 10;
        //    if (count % 10 > 0)
        //    {
        //        page++;
        //    }
        //    return page;
        //}
    }
}

