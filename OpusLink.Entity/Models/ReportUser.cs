using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class ReportUser
    {
        public int ReportUserID { get; set; }
        public int CreateByUserID { get; set; }
        public int TargetToUserID { get; set; }
        public string ReportUserContent { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User? CreateByUser { get; set; }
        public virtual User? TargetToUser { get; set; }
    }
}
