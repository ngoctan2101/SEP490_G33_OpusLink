using AutoMapper;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class WithdrawRequesMapper : Profile
    {
        public WithdrawRequesMapper() {
            CreateMap<WithdrawRequestDTO, WithdrawRequest>();
        


            CreateMap<WithdrawRequest, WithdrawResponseDTO>()
                  .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.User.UserName))
                    .ForMember(dest => dest.BankAccountInfor, src => src.MapFrom(x => x.User.BankAccountInfor))
                      .ForMember(dest => dest.BankName , src => src.MapFrom(x => x.User.BankName))
                      .ForMember(dest => dest.AmountUser, src => src.MapFrom(x => x.User.AmountMoney));

        }
    }
}
