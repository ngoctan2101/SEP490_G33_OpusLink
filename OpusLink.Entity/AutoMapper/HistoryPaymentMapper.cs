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
    public class HistoryPaymentMapper : Profile
    {
        public HistoryPaymentMapper()
        {
            CreateMap<HistoryPayment, HistoryPaymentDTO>()
                .ReverseMap();
        }

        
    }
}
