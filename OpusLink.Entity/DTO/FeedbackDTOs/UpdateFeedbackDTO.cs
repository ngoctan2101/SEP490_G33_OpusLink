using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.FeedbackDTOs
{
    public class UpdateFeedbackDTO
    {
        public int FeedbackUserID { get; set; }
        public decimal Star { get; set; }
        public string Content { get; set; }
    }
}
