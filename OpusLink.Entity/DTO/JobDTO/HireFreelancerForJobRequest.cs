using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class HireFreelancerForJobRequest
    {
        public int FreelancerId { get; set; }
        public int JobId { get; set; }
    }
}
