using AutoMapper;
using OpusLink.Entity.Models;
using OpusLink.Entity.Models.JOB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class JobProfile : Profile
    {
        public JobProfile() {
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
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status))
                //lay mot list category cua cai job nay
                .ForMember(dest => dest.Categoies, src => src.MapFrom(x => GetListOfGetCategoryResponse(x.JobAndCategories)))
                //Tinh xem job nay co bao nhieu offer
                .ForMember(dest => dest.NumberOfOffer, src => src.MapFrom(x => x.Offers.Count));
        }

        private List<GetCategoryResponse> GetListOfGetCategoryResponse(ICollection<JobAndCategory> jobAndCategories)
        {
            List<GetCategoryResponse> result = new List<GetCategoryResponse>();
            foreach(JobAndCategory jac in jobAndCategories)
            {
                result.Add(new GetCategoryResponse() { CategoryID= jac.CategoryID, CategoryName=jac.Category.CategoryName });
            }
            return result;
        }
    }
}
