using OpusLink.Entity.Models;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace OpusLink.Service.UserServices
{
    public interface IFreelancerAndSkillService
    {
        Task AddRangeAsync(List<FreelancerAndSkill> fasa);
        Task DeleteRangeAsync(List<FreelancerAndSkill> fasd);
        Task<List<FreelancerAndSkill>> getAllFASOfUser(int userID);
    }
    public class FreelancerAndSkillService : IFreelancerAndSkillService
    {
        private readonly OpusLinkDBContext _dbContext;
        public FreelancerAndSkillService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(List<FreelancerAndSkill> fasa)
        {
            await _dbContext.AddRangeAsync(fasa);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<FreelancerAndSkill> fasd)
        {
            _dbContext.RemoveRange(fasd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<FreelancerAndSkill>> getAllFASOfUser(int userID)
        {
            return await _dbContext.FreelancerAndSkills.Where(fas => fas.FreelancerID == userID).ToListAsync();
        }
        
    }
}
