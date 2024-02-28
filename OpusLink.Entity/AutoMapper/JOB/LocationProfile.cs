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
    public class LocationProfile : Profile
    {
        public LocationProfile() {
            CreateMap<Location, GetLocationResponse>()
                .ForMember(dest => dest.LocationId, src => src.MapFrom(x => x.LocationID))
                .ForMember(dest => dest.LocationName, src => src.MapFrom(x => x.LocationName));
        }
    }
}
