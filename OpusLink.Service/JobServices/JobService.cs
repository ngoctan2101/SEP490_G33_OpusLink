﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpusLink.Entity;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.JobServices
{
    public interface IJobService
    {
        List<Job> Search(Filter filter, bool forFreelancer, out int numberOfPage);
        List<Job> SearchById(Filter filter, out int numberOfPage);
        List<Job> GetAllJobRequested(Filter filter, out int numberOfPage);
        List<Job> GetAllJobRequested2();
        List<Job> GetJob();
        Task ApproveJob(int jobId);
        Task<Job> GetJobDetail(int jobId);
        Task<int> CreateNewJob(Job j);
        Task UpdateOnlyJobProperties(Job a);
        Task HireFreelancerForJob(int freelancerId, int jobId);
        Task CancelHireFreelancerForJob(int freelancerId, int jobId);
    }
    public class JobService : IJobService
    {
        private readonly OpusLinkDBContext _dbContext;
        public JobService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Job> GetAllJobRequested(Filter filter, out int numberOfPage)
        {
            List<Job> jobs = new List<Job>();
            jobs = _dbContext.Jobs
                .Include("Employer")
                .Where(j => j.Status == (int)JobStatusEnum.NotApprove)
                .OrderByDescending(j=>j.DateCreated)
                .ToList();
            jobs = jobs.Where(j => (filter.Statuses.Count == 0 ? true : filter.Statuses.Contains(j.Status)) &&
                                (filter.CategoryIDs.Count == 0 ? true : j.JobAndCategories.Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))) &&
                                (j.BudgetFrom <= filter.BudgetMax && j.BudgetTo >= filter.BudgetMin) &&
                                (j.DateCreated >= filter.DateMin && j.DateCreated <= filter.DateMax) &&
                                (filter.SearchStr.Length == 0 ? true : (j.JobTitle.Contains(filter.SearchStr) || j.JobContent.Contains(filter.SearchStr)))).ToList();
            // Calculate number of pages
            numberOfPage = jobs.Count / 6;
            if (jobs.Count % 6 > 0)
            {
                numberOfPage++;
            }
            // Return paginated result
            return jobs.Skip((filter.PageNumber - 1) * 6).Take(6).ToList();
        }
        public List<Job> GetAllJobRequested2()
        {
            List<Job> jobs = new List<Job>();
            jobs = _dbContext.Jobs
                .Include("Employer")
                .Where(j => j.Status == (int)JobStatusEnum.NotApprove)
                .ToList();


            return jobs.ToList();
        }
        public async Task ApproveJob(int jobId)
        {
            Job a = await _dbContext.Jobs.Where(j => j.JobID == jobId).FirstAsync();
            a.Status = (int)JobStatusEnum.Hiring;
            Notification n = new Notification()
            {
                NotificationID = 0,
                UserID = a.EmployerID,
                NotificationContent = "Job của bạn đã được duyệt",
                IsReaded = false,
                Link = "/JOB/EmployerViewJobDetailPage?JobId=" + a.JobID,
                NotificationDate = DateTime.Now
            };
            _dbContext.Notifications.Add(n);
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
        public List<Job> GetJob()
        {
            var jobs = _dbContext.Jobs
                .Include("Employer").ToList();
            return jobs;
        }

        public async Task<int> CreateNewJob(Job j)
        {
            var e = await _dbContext.Users.Where(u => u.Id == j.EmployerID).FirstOrDefaultAsync();
            e.AmountMoney -= 50000;
            await _dbContext.Jobs.AddAsync(j);
            await _dbContext.HistoryPayments.AddAsync(new HistoryPayment() { PaymentID = 0, UserID = j.EmployerID, Amount = 50000m, TransactionType = 4 , TransactionCode="", TransactionDate = DateTime.Now }); //phi post job
            await _dbContext.SaveChangesAsync();
            return j.JobID;
        }

        public async Task UpdateOnlyJobProperties(Job a)
        {
            Job b = await _dbContext.Jobs.Where(b => b.JobID == a.JobID).FirstAsync();
            b.JobTitle = a.JobTitle;
            b.JobContent = a.JobContent;
            b.LocationID = a.LocationID;
            b.BudgetFrom = a.BudgetFrom;
            b.BudgetTo = a.BudgetTo;
            b.Status = a.Status;
            b.EndHiringDate = a.EndHiringDate;
            if (b.Status != (int)JobStatusEnum.NotApprove)
            {
                var e = await _dbContext.Users.Where(u => u.Id == b.EmployerID).FirstOrDefaultAsync();
                e.AmountMoney -= 50000;
                await _dbContext.HistoryPayments.AddAsync(new HistoryPayment() { PaymentID = 0, UserID = b.EmployerID, Amount = 50000m, TransactionType = 4, TransactionCode = "" , TransactionDate = DateTime.Now }); //phi post job
            }
            b.Status = (int)JobStatusEnum.NotApprove;
            await _dbContext.SaveChangesAsync();
        }

        public List<Job> Search(Filter filter, bool forFreelancer, out int numberOfPage)
        {
            List<Job> jobsFinal = new List<Job>();
            numberOfPage = 0;
            try
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                if (filter.Statuses.Contains((int)JobStatusEnum.Hiring) && !filter.Statuses.Contains((int)JobStatusEnum.HiringExpried))
                {
                    var jobs = _dbContext.Jobs
                .Include("JobAndCategories")
                .Include("JobAndCategories.Category")
                .Include("Offers")
                .Include("Location")
                .Include("Employer")
                .Where(j => (filter.Statuses.Count == 0 ? true : filter.Statuses.Contains(j.Status)) &&
                                (filter.CategoryIDs.Count == 0 ? true : j.JobAndCategories.Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))) &&
                                (j.BudgetFrom <= filter.BudgetMax && j.BudgetTo >= filter.BudgetMin) &&
                                (j.DateCreated >= filter.DateMin && j.DateCreated <= filter.DateMax) &&
                                (filter.SearchStr.Length == 0 ? true : (j.JobTitle.Contains(filter.SearchStr) || j.JobContent.Contains(filter.SearchStr))));
                    jobs = jobs.Where(j => !(j.Status == (int)JobStatusEnum.Hiring && j.EndHiringDate < DateTime.Now));
                    if (forFreelancer)
                    {
                        jobs = jobs.Where(j => j.Status != (int)JobStatusEnum.NotApprove);
                    }
                    jobs = jobs.OrderByDescending(j => j.DateCreated);
                    jobsFinal = jobs.ToList();
                }
                else if (filter.Statuses.Contains((int)JobStatusEnum.HiringExpried) && !filter.Statuses.Contains((int)JobStatusEnum.Hiring))
                {
                    filter.Statuses.Add((int)JobStatusEnum.Hiring);
                    var jobs = _dbContext.Jobs
               .Include("JobAndCategories")
               .Include("JobAndCategories.Category")
               .Include("Offers")
               .Include("Location")
               .Include("Employer")
               .Where(j => (filter.Statuses.Count == 0 ? true : filter.Statuses.Contains(j.Status)) &&
                               (filter.CategoryIDs.Count == 0 ? true : j.JobAndCategories.Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))) &&
                               (j.BudgetFrom <= filter.BudgetMax && j.BudgetTo >= filter.BudgetMin) &&
                               (j.DateCreated >= filter.DateMin && j.DateCreated <= filter.DateMax) &&
                               (filter.SearchStr.Length == 0 ? true : (j.JobTitle.Contains(filter.SearchStr) || j.JobContent.Contains(filter.SearchStr))));
                    jobs = jobs.Where(j => j.EndHiringDate < DateTime.Now);
                    if (forFreelancer)
                    {
                        jobs = jobs.Where(j => j.Status != (int)JobStatusEnum.NotApprove);
                    }
                    jobs = jobs.OrderByDescending(j => j.DateCreated);
                    jobsFinal = jobs.ToList();
                }
                else
                {
                    var jobs = _dbContext.Jobs
               .Include("JobAndCategories")
               .Include("JobAndCategories.Category")
               .Include("Offers")
               .Include("Location")
               .Include("Employer")
               .Where(j => (filter.Statuses.Count == 0 ? true : filter.Statuses.Contains(j.Status)) &&
                               (filter.CategoryIDs.Count == 0 ? true : j.JobAndCategories.Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))) &&
                               (j.BudgetFrom <= filter.BudgetMax && j.BudgetTo >= filter.BudgetMin) &&
                               (j.DateCreated >= filter.DateMin && j.DateCreated <= filter.DateMax) &&
                               (filter.SearchStr.Length == 0 ? true : (j.JobTitle.Contains(filter.SearchStr) || j.JobContent.Contains(filter.SearchStr))));
                    if (forFreelancer)
                    {
                        jobs = jobs.Where(j => j.Status != (int)JobStatusEnum.NotApprove);
                    }
                    jobs = jobs.OrderByDescending(j => j.DateCreated);
                    jobsFinal = jobs.ToList();
                }

                // Calculate number of pages
                numberOfPage = jobsFinal.Count / 6;
                if (jobsFinal.Count % 6 > 0)
                {
                    numberOfPage++;
                }

                //sw.Stop();
                //Console.WriteLine("Elapsed={0}", sw.Elapsed);
                // Return paginated result
                return jobsFinal.Skip((filter.PageNumber - 1) * 6).Take(6).ToList();
            }
            catch (Exception es)
            {
                return null;
            }

        }

        public List<Job> SearchById(Filter filter, out int numberOfPage)
        {
            List<Job> jobs = new List<Job>();
            numberOfPage = 0;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                jobs = _dbContext.Jobs
            .Include("JobAndCategories")
            .Include("JobAndCategories.Category")
            .Include("Offers")
            .Include("Location")
            .Include("Employer")
            .Where(j => j.EmployerID == filter.UserId &&
                            (filter.Statuses.Count == 0 ? true : filter.Statuses.Contains(j.Status)) &&
                            (filter.CategoryIDs.Count == 0 ? true : j.JobAndCategories.Any(jac => filter.CategoryIDs.Contains(jac.CategoryID))) &&
                            (j.BudgetFrom <= filter.BudgetMax && j.BudgetTo >= filter.BudgetMin) &&
                            (j.DateCreated >= filter.DateMin && j.DateCreated <= filter.DateMax) &&
                            (filter.SearchStr.Length == 0 ? true : (j.JobTitle.Contains(filter.SearchStr) || j.JobContent.Contains(filter.SearchStr))))
            .OrderByDescending(j=>j.DateCreated)
            .ToList();



                // Calculate number of pages
                numberOfPage = jobs.Count / 6;
                if (jobs.Count % 6 > 0)
                {
                    numberOfPage++;
                }

                sw.Stop();
                Console.WriteLine("Elapsed={0}", sw.Elapsed);
                // Return paginated result
                return jobs.Skip((filter.PageNumber - 1) * 6).Take(6).ToList();
            }
            catch (Exception es)
            {
                return null;
            }
        }

        public async Task HireFreelancerForJob(int freelancerId, int jobId)
        {
            Job b = await _dbContext.Jobs.Where(b => b.JobID == jobId).FirstAsync();
            b.FreelancerID=freelancerId;
            b.Status = (int)JobStatusEnum.Hired;
            Notification n = new Notification()
            {
                NotificationID = 0,
                UserID = freelancerId,
                NotificationContent = "Bạn đã được nhận vào job",
                IsReaded = false,
                Link = "/JOB/FreelancerViewJobDetail?JobId=" + jobId,
                NotificationDate = DateTime.Now
            };
            _dbContext.Notifications.Add(n);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelHireFreelancerForJob(int freelancerId, int jobId)
        {
            Job b = await _dbContext.Jobs.Where(b => b.JobID == jobId).FirstAsync();
            b.FreelancerID = null;
            b.Status = (int)JobStatusEnum.Hiring;
            Notification n = new Notification()
            {
                NotificationID = 0,
                UserID = freelancerId,
                NotificationContent = "Bạn đã bị Employer từ chối",
                IsReaded = false,
                Link = "/JOB/FreelancerViewJobDetail?JobId=" + jobId,
                NotificationDate = DateTime.Now
            };
            _dbContext.Notifications.Add(n);
            await _dbContext.SaveChangesAsync();
        }
    }
}

