using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO
{
    public class ChatDTO
    {
        public int ChatBoxID { get; set; }
        public int EmployerID { get; set; }
        public int FreelancerID { get; set; }
        public bool IsViewed { get; set; }
        public string EmployerName { get; set; }
        public string FreelancerName { get; set; }
    }
}
