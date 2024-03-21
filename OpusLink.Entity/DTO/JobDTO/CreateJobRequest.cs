using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class CreateJobRequest
    {
        public string JobTitle { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public string JobContent { get; set; }
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }
        public int LocationId { get; set; }
        public int EmployerId { get; set; }
        public DateTime EndHiringDate { get; set; }
        public bool IsPublicMilestone { get; set; }
        public bool IsFreelancerConfirm { get; set; }
    }
}
