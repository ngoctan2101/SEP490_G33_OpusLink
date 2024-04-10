using OpusLink.Entity.Models;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Shared.Enums;

namespace OpusLink.Service.MSServices
{
    public interface IMilestoneService
    {
        Task AcceptPlanOrNot(int jobID, bool accepted);
        Task CreateMilestone(Milestone ms);
        Task DeleteMilestone(int milestoneID);
        Task<List<Milestone>> GetAllMilestoneByJobID(int jobID);
        Task<Job> GetThisJob(int jobID);
        Task<bool> RequestChangeStatus(int milestoneId, int jobId, int status);
        Task<bool> RequestDoneAMilestone(int milestoneId, int jobId);
        Task<bool> RequestExtendDeadline(int milestoneId, int jobId, DateTime newDeadline);
        Task<bool> RequestFailJob(int milestoneId, int jobId);
        Task RequestFreelancerAcceptPlan(int jobID, DateTime deadlineAccept);
        Task<bool> RequestGetBackMoney(int milestoneId, int jobId);
        Task<bool> RequestPutMoney(int milestoneId, int jobId);
        Task UpdateMilestone(CreateMilestoneRequest ms);
    }
    public class MilestoneService : IMilestoneService
    {
        private readonly OpusLinkDBContext _dbContext;
        public MilestoneService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AcceptPlanOrNot(int jobID, bool accepted)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobID).FirstOrDefaultAsync();
            if (accepted)
            {
                j.EmployerDoneEditMilestone = true;
                j.IsFreelancerConfirm = true;
            }
            else
            {
                j.EmployerDoneEditMilestone = false;
                j.IsFreelancerConfirm = false;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateMilestone(Milestone ms)
        {
            _dbContext.Milestones.Add(ms);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMilestone(int milestoneID)
        {
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneID).FirstOrDefaultAsync();
            _dbContext.Remove(m);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Milestone>> GetAllMilestoneByJobID(int jobID)
        {
            return await _dbContext.Milestones
                .Where(m => m.JobID == jobID)
                .ToListAsync();
        }

        public async Task<Job> GetThisJob(int jobID)
        {
            return await _dbContext.Jobs
                .Where(m => m.JobID == jobID)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RequestChangeStatus(int milestoneId, int jobId, int status)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobId).Include("Milestones").FirstOrDefaultAsync();
            User freelancer = await _dbContext.Users.Where(u => u.Id == j.FreelancerID).FirstOrDefaultAsync();
            User employer = await _dbContext.Users.Where(u => u.Id == j.EmployerID).FirstOrDefaultAsync();
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneId).FirstOrDefaultAsync();
            if (m.Status == (int)MilestoneStatusEnum.MoneyPutted && status == (int)MilestoneStatusEnum.Completed)
            {
                freelancer.AmountMoney += 0.8m * m.AmountToPay;
                m.Status = status;
            }
            else if (m.Status == (int)MilestoneStatusEnum.EmployerRejected && status == (int)MilestoneStatusEnum.Completed)
            {
                freelancer.AmountMoney += 0.8m * m.AmountToPay;
                m.Status = status;
            }
            else if(m.Status == (int)MilestoneStatusEnum.MoneyPutted && status == (int)MilestoneStatusEnum.EmployerRejected)
            {
                m.IsFreelancerDone = false;
                m.Status = status;
            }
            else if (m.Status == (int)MilestoneStatusEnum.EmployerRejected && status == (int)MilestoneStatusEnum.EmployerRejected)
            {
                m.IsFreelancerDone = false;
                m.Status = status;
            }
            else if(status == (int)MilestoneStatusEnum.Failed)
            {
                //chuyen trang thai ms thanh fail
                m.Status = (int)MilestoneStatusEnum.Failed;
                //tra tien cua ms nay cho E
                employer.AmountMoney += m.AmountToPay;
                
            }
            else
            {
                //hot hon hot
                m.Status = status;
            }
            //neu da het ms thi job se chuyen thanh complete
            if (NoMsLeft(j))
            {
                j.Status = (int)JobStatusEnum.Completed;
                //neu ko co ms nao bi fail thi F nhan duoc 20% tat ca nhung ms completed
                if (AnyMsFail(j) == false)
                {
                    freelancer.AmountMoney += 0.19m * TotalMoneyOfMsCompleted(j);
                }
                //new co 1 ms bi fail thi E nhan duoc 20% tat ca nhung ms completed
                else
                {
                    employer.AmountMoney += 0.19m * TotalMoneyOfMsCompleted(j);
                }
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool NoMsLeft(Job? j)
        {
            foreach (var m in j.Milestones)
            {
                if (m.Status == (int)MilestoneStatusEnum.MoneyPutted || m.Status == (int)MilestoneStatusEnum.EmployerRejected)
                {
                    return false;
                }
            }
            return true;
        }

        private bool AnyMsFail(Job j)
        {
            foreach(var m in j.Milestones)
            {
                if(m.Status!=(int)MilestoneStatusEnum.Completed) {
                    return true;
                }
            }
            return false;
        }

        private decimal TotalMoneyOfMsCompleted(Job j)
        {
            decimal totalMoney = 0;
            foreach (var m in j.Milestones)
            {
                if (m.Status == (int)MilestoneStatusEnum.Completed)
                {
                    totalMoney += m.AmountToPay;
                }
            }
            return totalMoney;
        }

        public async Task<bool> RequestDoneAMilestone(int milestoneId, int jobId)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstOrDefaultAsync();
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneId).FirstOrDefaultAsync();
            if (m.Deadline < DateTime.Now)
            {
                return false;
            }
            if (m.Status == (int)MilestoneStatusEnum.MoneyPutted)
            {
                m.IsFreelancerDone = true;
                await _dbContext.SaveChangesAsync();
                return true;

            }
            if (m.Status == (int)MilestoneStatusEnum.EmployerRejected)
            {
                m.IsFreelancerDone = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RequestExtendDeadline(int milestoneId, int jobId, DateTime newDeadline)
        {
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneId).FirstOrDefaultAsync();
            m.Deadline=newDeadline;
            m.IsFreelancerDone = false;
            m.Status = (int)MilestoneStatusEnum.EmployerRejected;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task RequestFreelancerAcceptPlan(int jobID, DateTime deadlineAccept)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobID).FirstOrDefaultAsync();
            j.DeadlineFreelancerConfirm = deadlineAccept;
            j.EmployerDoneEditMilestone = true;
            j.IsFreelancerConfirm = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RequestGetBackMoney(int milestoneId, int jobId)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstOrDefaultAsync();
            User u = await _dbContext.Users.Where(u => u.Id == j.EmployerID).FirstOrDefaultAsync();
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneId).FirstOrDefaultAsync();
            if (m.JobID != jobId)
            {
                return false;
            }
            if (j.Status == (int)JobStatusEnum.Hired && (j.IsFreelancerConfirm == true || (j.EmployerDoneEditMilestone == true&&j.DeadlineFreelancerConfirm>DateTime.Now&&j.IsFreelancerConfirm==false)))
            {
                return false;
            }
            u.AmountMoney = u.AmountMoney + m.AmountToPay;
            m.Status = (int)MilestoneStatusEnum.EmployerCreated;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RequestPutMoney(int milestoneId, int jobId)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstOrDefaultAsync();
            User u = await _dbContext.Users.Where(u => u.Id == j.EmployerID).FirstOrDefaultAsync();
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == milestoneId).FirstOrDefaultAsync();
            if (m.JobID != jobId)
            {
                return false;
            }
            if (u.AmountMoney < m.AmountToPay)
            {
                return false;
            }
            if (m.Status != (int)MilestoneStatusEnum.EmployerCreated)
            {
                return false;
            }
            u.AmountMoney = u.AmountMoney - m.AmountToPay;
            m.Status = (int)MilestoneStatusEnum.MoneyPutted;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateMilestone(CreateMilestoneRequest ms)
        {
            Milestone m = await _dbContext.Milestones.Where(m => m.MilestoneID == ms.MilestoneID).FirstOrDefaultAsync();
            m.MilestoneContent = ms.MilestoneContent;
            m.Deadline = ms.Deadline;
            m.AmountToPay = ms.AmountToPay;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RequestFailJob(int milestoneId, int jobId)
        {
            Job j = await _dbContext.Jobs.Where(j => j.JobID == jobId).Include("Milestones").FirstOrDefaultAsync();
            User employer = await _dbContext.Users.Where(u => u.Id == j.EmployerID).FirstOrDefaultAsync();
            j.Status = (int)JobStatusEnum.Failed;
            //E nhan lai 20% cua nhung ms da Completed. 100% nhung ms MoneyPutted
            //100% nhung ms EmployerRejected
            Decimal total = 0;
            foreach (var m in j.Milestones)
            {
                if (m.Status == (int)MilestoneStatusEnum.Completed)
                {
                    total += m.AmountToPay *0.19m;
                }else if(m.Status == (int)MilestoneStatusEnum.MoneyPutted)
                {
                    total += m.AmountToPay;
                }else if(m.Status == (int)MilestoneStatusEnum.EmployerRejected)
                {
                    total += m.AmountToPay;
                }
            }
            employer.AmountMoney += total;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
