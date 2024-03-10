using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class FeedbackUser
    {
        public int FeedbackUserID { get; set; }
        public int JobID { get; set; }
        public int CreateByUserID { get; set; }
        public int TargetToUserID { get; set; }
        public int Star { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User? CreateByUser { get; set; }
        public virtual User? TargetToUser { get; set; }
        public virtual Job? Job { get; set; }
    }
}
