using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string NotificationContent { get; set; }
        public bool IsReaded { get; set; }
        public string Link { get; set; }
        public DateTime NotificationDate { get; set; }

        public virtual User? User { get; set; }
    }
}
