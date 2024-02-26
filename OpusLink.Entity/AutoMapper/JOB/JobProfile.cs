using AutoMapper;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper.JOB
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, GetJobResponse>()
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                .ForMember(dest => dest.JobTitle, src => src.MapFrom(x => x.JobTitle))
                //lay thong tin employer
                .ForMember(dest => dest.EmployerID, src => src.MapFrom(x => x.EmployerID))
                .ForMember(dest => dest.EmployerName, src => src.MapFrom(x => x.Employer.UserName))
                .ForMember(dest => dest.EmployerStarMedium, src => src.MapFrom(x => x.Employer.StarMedium))
                .ForMember(dest => dest.FreelancerID, src => src.MapFrom(x => x.FreelancerID))
                .ForMember(dest => dest.JobContent, src => src.MapFrom(x => x.JobContent))
                .ForMember(dest => dest.BudgetFrom, src => src.MapFrom(x => x.BudgetFrom))
                .ForMember(dest => dest.BudgetTo, src => src.MapFrom(x => x.BudgetTo))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom(x => x.DateCreated))
                .ForMember(dest => dest.LocationID, src => src.MapFrom(x => x.LocationID))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(x => x.Location.LocationName))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status))
                //lay mot list category cua cai job nay
                .ForMember(dest => dest.Categoies, src => src.MapFrom(x => GetListOfGetCategoryResponse(x.JobAndCategories)))
                //Tinh xem job nay co bao nhieu offer
                .ForMember(dest => dest.NumberOfOffer, src => src.MapFrom(x => x.Offers.Count));

            CreateMap<Job, GetJobDetailResponse>()
                .ForMember(dest => dest.JobId, src => src.MapFrom(x => x.JobID))
                .ForMember(dest => dest.JobTitle, src => src.MapFrom(x => x.JobTitle))
                .ForMember(dest => dest.JobContent, src => src.MapFrom(x => x.JobContent))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dest => dest.EmployerId, src => src.MapFrom(x => x.EmployerID))
                .ForMember(dest => dest.FreelancerId, src => src.MapFrom(x => x.FreelancerID))
                .ForMember(dest => dest.EmployerName, src => src.MapFrom(x => x.Employer.UserName))
                .ForMember(dest => dest.EmployerStarMedium, src => src.MapFrom(x => x.Employer.StarMedium))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom(x => x.DateCreated))
                .ForMember(dest => dest.BudgetMin, src => src.MapFrom(x => x.BudgetFrom))
                .ForMember(dest => dest.BudgetMax, src => src.MapFrom(x => x.BudgetTo))
                .ForMember(dest => dest.Location, src => src.MapFrom(x => x.Location.LocationName))
                .ForMember(dest => dest.LocationId, src => src.MapFrom(x => x.Location.LocationID))
                .ForMember(dest => dest.TotalOffer, src => src.MapFrom(x => TotalOffer(x.Offers)))
                .ForMember(dest => dest.AverageCost, src => src.MapFrom(x => AverageCost(x.Offers)))
                .ForMember(dest => dest.AverageTime, src => src.MapFrom(x => AverageTime(x.Offers)))
                .ForMember(dest => dest.Categories, src => src.MapFrom(x => GetListOfGetCategoryResponse(x.JobAndCategories)));

            CreateMap<CreateJobRequest, Job>()
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => 0))
                .ForMember(dest => dest.JobTitle, src => src.MapFrom(x => x.JobTitle))
                .ForMember(dest => dest.EmployerID, src => src.MapFrom(x => x.EmployerId))
                .ForMember(dest => dest.JobContent, src => src.MapFrom(x => x.JobContent))
                .ForMember(dest => dest.BudgetFrom, src => src.MapFrom(x => x.BudgetMin))
                .ForMember(dest => dest.BudgetTo, src => src.MapFrom(x => x.BudgetMax))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.LocationID, src => src.MapFrom(x => x.LocationId))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => (int)JobStatusEnum.NotApprove));

            CreateMap<PutJobRequest, Job>()
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                .ForMember(dest => dest.JobTitle, src => src.MapFrom(x => x.JobTitle))
                .ForMember(dest => dest.EmployerID, src => src.MapFrom(x => x.EmployerID))
                .ForMember(dest => dest.JobContent, src => src.MapFrom(x => x.JobContent))
                .ForMember(dest => dest.BudgetFrom, src => src.MapFrom(x => x.BudgetFrom))
                .ForMember(dest => dest.BudgetTo, src => src.MapFrom(x => x.BudgetTo))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom(x => x.DateCreated))
                .ForMember(dest => dest.LocationID, src => src.MapFrom(x => x.LocationID))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status));

        }

        private object AverageTime(ICollection<Offer> offers)
        {
            if (offers.Count == 0)
            {
                return 0;
            }
            return (int)Math.Round(offers.Average(o => o.ExpectedDays));
        }

        private object AverageCost(ICollection<Offer> offers)
        {
            if (offers.Count == 0)
            {
                return 0;
            }
            return Math.Round(offers.Average(o => o.ProposedCost), 0);
        }

        private object TotalOffer(ICollection<Offer> offers)
        {
            if (offers.Count == 0)
            {
                return 0;
            }
            return offers.Count;
        }

        private List<GetCategoryResponse> GetListOfGetCategoryResponse(ICollection<JobAndCategory> jobAndCategories)
        {
            List<GetCategoryResponse> result = new List<GetCategoryResponse>();
            foreach (JobAndCategory jac in jobAndCategories)
            {
                result.Add(new GetCategoryResponse() { CategoryID = jac.CategoryID, CategoryName = jac.Category.CategoryName });
            }
            return result;
        }
    }
}
