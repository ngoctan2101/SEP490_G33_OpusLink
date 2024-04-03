using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.MSDTO
{
    public class RequestChangeStatus
    {
        public int MilestoneId { get; set; }
        public int JobId { get; set; }
        public int Status { get; set; }
    }
}
