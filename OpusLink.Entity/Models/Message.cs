using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public int ChatBoxID { get; set; }
        public bool FromEmployer { get; set; }
        public DateTime DateCreated { get; set; }
        public string MessageContent { get; set; }
        public virtual ChatBox? ChatBox { get; set; }
    }
}
