using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class ReportJob
    {
        public int ReportJobID { get; set; }
        public int TargetToJob { get; set; }
        public int CreateByFreelancer { get; set; }
        public string ReportJobContent { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual User? Freelancer { get; set; }
        public virtual Job? Job { get; set; }
    }
}
