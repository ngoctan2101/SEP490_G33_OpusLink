using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class GetOfferResponse
    {
        public int OfferID { get; set; }
        public int FreelancerID { get; set; }
        public string EmployerProfileImage { get; set; } = "";
        public byte[] FreelancerImageBytes { get; set; }
        public int JobID { get; set; }
        public DateTime DateOffer { get; set; }
        public decimal ProposedCost { get; set; }
        public int ExpectedDays { get; set; }
        public string SelfIntroduction { get; set; }
        public string EstimatedPlan { get; set; }
        public GetJobResponse GetJobResponse { get; set; }
    }
}
