using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.MSDTO
{
    public class RequestExtendDeadline
    {
        public int MilestoneId { get; set; }
        public DateTime NewDeadline { get; set; }
        public int JobId { get; set; }
    }
}
