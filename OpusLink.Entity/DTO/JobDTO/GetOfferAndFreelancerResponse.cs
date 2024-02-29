using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class GetOfferAndFreelancerResponse
    {
        public int OfferID { get; set; }
        public int FreelancerID { get; set; }
        public DateTime DateOffer { get; set; }
        public decimal ProposedCost { get; set; }
        public int ExpectedDays { get; set; }
        public string SelfIntroduction { get; set; }
        public string EstimatedPlan { get; set; }
        public string FreelancerName { get; set; }
        public int FreelancerStarMedium { get; set; }
        public string FreelancerProfileImage { get; set; } = "";
        public int NumberOfJobDone { get; set; }
        
        public List<string> Skills { get; set; }=new List<string>();
        public byte[] FreelancerImageBytes { get; set; } 
        
    }
}
