using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class PutJobRequest
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
        public List<Int32> CategoryIDs { get; set; }=new List<Int32>();
    }
}
