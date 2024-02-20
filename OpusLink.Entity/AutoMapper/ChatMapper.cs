using AutoMapper;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class ChatMapper : Profile
    {
            public ChatMapper()
        {
            CreateMap<ChatBox, ChatDTO>()
                .ForMember(x => x.EmployerName, x => x.MapFrom(x => x.Employer.UserName))
                .ForMember(x => x.FreelancerName, x => x.MapFrom(x => x.Freelancer.UserName))
                

                .ReverseMap();
        }
    }
}
