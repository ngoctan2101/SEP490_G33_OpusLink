using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class ChatBox
    {
        public int ChatBoxID { get; set; }
        public int JobID { get; set; }
        public int EmployerID { get; set; }
        public int FreelancerID { get; set; }
        public bool IsViewed { get; set; }
        public virtual User? Freelancer { get; set; }
        public virtual User? Employer { get; set; }
        public virtual Job? Job { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
