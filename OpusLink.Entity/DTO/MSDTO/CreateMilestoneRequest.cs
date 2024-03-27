using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.MSDTO
{
    public class CreateMilestoneRequest
    {
        public int JobID { get; set; }
        public string MilestoneContent { get; set; }
        public DateTime Deadline { get; set; }
        public decimal AmountToPay { get; set; }
    }
}
