using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Milestone
    {
        public int MilestoneID { get; set; }
        public int JobID { get; set; }
        public string MilestoneContent { get; set; }
        //public string? FilePathFreelancerUpload { get; set; }
        public DateTime Deadline { get; set; }
        public decimal AmountToPay { get; set; }
        public int Status { get; set; }
        public DateTime DateEdited { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Job? Job { get; set; }
    }
}
