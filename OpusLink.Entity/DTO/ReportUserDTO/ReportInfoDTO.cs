using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.ReportUserDTO
{
    public class ReportInfoDTO
    {
        public int CreateByUserID { get; set; }
        public string CreateByUserName { get; set; }
        public string DateCreated { get; set; }
        public string ReportUserContent { get; set; }
    }
}
