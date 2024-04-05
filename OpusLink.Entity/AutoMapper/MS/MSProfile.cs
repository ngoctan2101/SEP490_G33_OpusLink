using AutoMapper;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.DTO.MSDTO;
using OpusLink.Entity.Models;
using OpusLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper.MS
{
    public class MSProfile : Profile
    {
        public MSProfile()
        {
            CreateMap<Milestone, GetMilestoneResponse>()
                .ForMember(dest => dest.MilestoneID, src => src.MapFrom(x => x.MilestoneID))
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                //lay thong tin employer
                .ForMember(dest => dest.MilestoneContent, src => src.MapFrom(x => x.MilestoneContent))
                .ForMember(dest => dest.Deadline, src => src.MapFrom(x => x.Deadline))
                .ForMember(dest => dest.AmountToPay, src => src.MapFrom(x => x.AmountToPay))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dest => dest.IsFreelancerDone, src => src.MapFrom(x => x.IsFreelancerDone));
            CreateMap<CreateMilestoneRequest, Milestone>()
                .ForMember(dest => dest.MilestoneID, src => src.MapFrom(x => 0))
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                .ForMember(dest => dest.MilestoneContent, src => src.MapFrom(x => x.MilestoneContent))
                .ForMember(dest => dest.Deadline, src => src.MapFrom(x => x.Deadline))
                .ForMember(dest => dest.AmountToPay, src => src.MapFrom(x => x.AmountToPay))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => (int)MilestoneStatusEnum.EmployerCreated))
                .ForMember(dest => dest.IsFreelancerDone, src => src.MapFrom(x => false));

        }
    }
}
