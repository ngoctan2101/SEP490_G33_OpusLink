using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class SaveJob
    {
        public int SaveJobID { get; set; }
        public int JobID { get; set; }
        public int FreelancerID { get; set; }

        public virtual User? Freelancer { get; set; }
        public virtual Job? Job { get; set; }
    }
}
