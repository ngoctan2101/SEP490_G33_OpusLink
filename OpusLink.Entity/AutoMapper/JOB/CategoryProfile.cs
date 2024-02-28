using AutoMapper;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusLink.Entity.DTO.JobDTO;

namespace OpusLink.Entity.AutoMapper.JOB
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoryResponse>()
                .ForMember(dest => dest.CategoryID, src => src.MapFrom(x => x.CategoryID))
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.CategoryName))
                .ForMember(dest => dest.NumberOfJob, src => src.MapFrom(x => x.JobAndCategories.Count))
                .ForMember(dest => dest.ParentId, src => src.MapFrom(x => x.CategoryParentID))
                .ForMember(dest => dest.ParentName, src => src.MapFrom(x => x.CategoryParent.CategoryName))
                .ReverseMap();
            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(dest => dest.CategoryID, src => src.MapFrom(x => 0))
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.CategoryName))
                .ForMember(dest => dest.CategoryParentID, src => src.MapFrom(x => x.ParentID))
                .ReverseMap();
            CreateMap<PutCategoryRequest, Category>()
                .ForMember(dest => dest.CategoryID, src => src.MapFrom(x => x.CategoryId))
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.CategoryName))
                .ForMember(dest => dest.CategoryParentID, src => src.MapFrom(x => x.ParentID))
                .ReverseMap();
        }

    }
}
