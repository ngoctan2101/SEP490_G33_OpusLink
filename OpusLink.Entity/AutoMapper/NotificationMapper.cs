using AutoMapper;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.NotificationDTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<Notification, NotificationDTO>().ReverseMap();
                
        }
    }
}
