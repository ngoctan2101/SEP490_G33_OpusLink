using AutoMapper;
using OpusLink.Entity.Models.JOB;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() {
            CreateMap<Category, GetCategoryResponse>()
                .ForMember(dest => dest.CategoryID, src => src.MapFrom(x => x.CategoryID))
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.CategoryName))
                .ForMember(dest => dest.NumberOfJob, src => src.MapFrom(x => x.JobAndCategories.Count));
        }

    }
}
