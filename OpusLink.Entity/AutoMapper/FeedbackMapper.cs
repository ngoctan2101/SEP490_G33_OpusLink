using AutoMapper;
using OpusLink.Entity.DTO.FeedbackDTO;
using OpusLink.Entity.DTO.FeedbackDTOs;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.AutoMapper
{
	public class FeedbackMapper : Profile
	{
		public FeedbackMapper()
		{
			CreateMap<FeedbackUser, FeebackDTO>();


		}
	}
}

