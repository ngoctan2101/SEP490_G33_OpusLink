using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class GetSaveJobResponse
    {
        public int SaveJobID { get; set; }
        public int JobID { get; set; }
        public int FreelancerID { get; set; }
        public GetJobResponse GetJobResponse { get; set; }
    }
}
