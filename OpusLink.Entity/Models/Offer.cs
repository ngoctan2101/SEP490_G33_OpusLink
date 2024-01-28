using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Offer
    {
        public int OfferID { get; set; }
        public int FreelancerID { get; set; }
        public int JobID { get; set; }
        public DateTime DateOffer { get; set; }
        public decimal ProposedCost { get; set; }
        public int ExpectedDays { get; set; }
        public string SelfIntroduction { get; set; }
        public string EstimatedPlan { get; set; }
        public virtual Job? Job { get; set; }
        public virtual User? Freelancer { get; set; }
    }
}
