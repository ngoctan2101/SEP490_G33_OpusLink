using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO
{
    public class ReportAccountDTO
    {
        public int CreateByUserID { get; set; }
        public int TargetToUserID { get; set; }
        public string ReportUserContent { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
