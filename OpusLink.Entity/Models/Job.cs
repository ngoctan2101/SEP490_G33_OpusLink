using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Job
    {
        public int JobID { get; set; }
        public string? JobTitle { get; set; }
        public int EmployerID { get; set; }
        public int? FreelancerID { get; set; }
        public string? JobContent { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? LocationID { get; set; }
        public int Status { get; set; }
        public virtual ICollection<JobAndCategory> JobAndCategories { get; set; } = new List<JobAndCategory>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public virtual ICollection<ReportJob> ReportJobs { get; set; } = new List<ReportJob>();
        public virtual ICollection<SaveJob> SaveJobs { get; set; } = new List<SaveJob>();

        //chua co SaveJob va ReportJob
        public virtual Location? Location { get; set; }
        public virtual User? Freelancer { get; set; }
        public virtual User? Employer { get; set; }
    }
}
