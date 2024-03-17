using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO
{
    public class CreateMessageDTO
    {
        public int ChatBoxID { get; set; }
        public bool FromEmployer { get; set; }
        public string MessageContent { get; set; }
    }
}
