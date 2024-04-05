using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.MSDTO
{
    public class RequestFreelancerAcceptPlan
    {
        public int JobID { get; set; }
        public DateTime DeadlineAccept { get; set; }
    }
}
