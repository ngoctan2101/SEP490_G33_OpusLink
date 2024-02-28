using AutoMapper;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper.JOB
{
    public class SaveJobProfile :Profile
    {
        public SaveJobProfile() {
            CreateMap<SaveJob, GetSaveJobResponse>()
                .ForMember(dest => dest.SaveJobID, src => src.MapFrom(x => x.SaveJobID))
                .ForMember(dest => dest.FreelancerID, src => src.MapFrom(x => x.FreelancerID))
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                .ReverseMap();
            CreateMap<CreateSaveJobRequest,SaveJob>()
                .ForMember(dest=>dest.SaveJobID,src=>src.MapFrom(x=>0))
                .ForMember(dest=>dest.FreelancerID,src=>src.MapFrom(x=>x.FreelancerID))
                .ForMember(dest=>dest.JobID,src=>src.MapFrom(x=>x.JobID))
                .ReverseMap();
        }
    }
}
