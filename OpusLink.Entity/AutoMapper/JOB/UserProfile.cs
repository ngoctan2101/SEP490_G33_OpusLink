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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<PutUserRequest, User>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address))
                .ForMember(dest => dest.FullNameOnIDCard, src => src.MapFrom(x => x.FullNameOnIDCard))
                .ForMember(dest => dest.IDNumber, src => src.MapFrom(x => x.IDNumber))
                .ForMember(dest => dest.Dob, src => src.MapFrom(x => x.Dob))
                .ForMember(dest => dest.Introduction, src => src.MapFrom(x => x.Introduction))
                .ForMember(dest => dest.BankName, src => src.MapFrom(x => x.BankName))
                .ForMember(dest => dest.BankAccountInfor, src => src.MapFrom(x => x.BankAccountInfor))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status));
        }
    }
}
