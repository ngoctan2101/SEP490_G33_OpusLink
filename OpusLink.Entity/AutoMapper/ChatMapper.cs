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
                .ForMember(x => x.ProfilePicture, x => x.MapFrom(x => x.Employer.ProfilePicture))
                .ForMember(x => x.FreelancerName, x => x.MapFrom(x => x.Freelancer.UserName))
                .ForMember(x => x.ProfilePicture, x => x.MapFrom(x => x.Freelancer.ProfilePicture))
                .ForMember(x => x.NewEstMessage, x => x.MapFrom(x => NewEstMessage(x.Messages)));
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("HH:mm"))); 
        }

        private string NewEstMessage(ICollection<Message> messages)
        {
            if(messages.Count == 0)
            {
                return "";
            }
            List<Message> messagesList = messages.OrderByDescending(o => o.DateCreated).ToList();
            return (messagesList[0].MessageContent);
        }
    }
}
