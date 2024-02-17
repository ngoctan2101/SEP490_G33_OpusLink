using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models.JOB
{
    public class Filter
    {
        public List<int> CategoryIDs { get; set; }
        public List<int> Statuses { get; set; }
        public String SearchStr { get; set; }
        public Decimal BudgetMin { get; set; }
        public Decimal BudgetMax { get; set;}
        public DateTime DateMin { get; set; }
        public DateTime DateMax { get; set; }
        public int PageNumber { get; set; }
    }
}
