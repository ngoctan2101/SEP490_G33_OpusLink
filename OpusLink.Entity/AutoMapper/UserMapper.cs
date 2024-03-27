using AutoMapper;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.TotalReport, opt => opt.MapFrom(src => src.ReportUsersAsATargeter.Count()))
                .ReverseMap();

            CreateMap<Skill, SkillDTO>()
                .ForMember(dest => dest.SkillParentName, opt => opt.MapFrom(src => src.SkillParent != null ? src.SkillParent.SkillName : null));
            
            CreateMap<ReportUser, ReportAccountDTO>()
                .ReverseMap();
        }
    }
}
