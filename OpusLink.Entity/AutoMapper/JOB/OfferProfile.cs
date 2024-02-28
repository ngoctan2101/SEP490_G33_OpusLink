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
    public class OfferProfile :Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, GetOfferResponse>()
                .ForMember(dest => dest.OfferID, src => src.MapFrom(x => x.OfferID))
                .ForMember(dest => dest.DateOffer, src => src.MapFrom(x => x.DateOffer))
                .ForMember(dest => dest.FreelancerID, src => src.MapFrom(x => x.FreelancerID))
                .ForMember(dest => dest.JobID, src => src.MapFrom(x => x.JobID))
                .ForMember(dest => dest.ProposedCost, src => src.MapFrom(x => x.ProposedCost))
                .ForMember(dest => dest.ExpectedDays, src => src.MapFrom(x => x.ExpectedDays))
                .ForMember(dest => dest.SelfIntroduction, src => src.MapFrom(x => x.SelfIntroduction))
                .ForMember(dest => dest.EstimatedPlan, src => src.MapFrom(x => x.EstimatedPlan))
                .ReverseMap();
        }
    }
}
