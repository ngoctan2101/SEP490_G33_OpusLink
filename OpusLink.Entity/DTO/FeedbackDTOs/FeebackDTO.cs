using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.FeedbackDTOs
{
	public class FeebackDTO
	{
		public int FeedbackUserID { get; set; }
		public int JobID { get; set; }
		public int CreateByUserID { get; set; }
		public int TargetToUserID { get; set; }
		public decimal Star { get; set; }
		public string Content { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
