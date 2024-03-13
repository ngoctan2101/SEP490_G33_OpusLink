using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.ReportUserDTO
{
    public class ReportUserDTO
    {
        public string CreateByUserName { get; set; }
        public string ReportUserContent { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual User? CreateByUser { get; set; }
    }
}
