using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.NotificationDTO
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string NotificationContent { get; set; }
        public bool IsReaded { get; set; }
        public string Link { get; set; }
        public DateTime NotificationDate { get; set; }

        //fivecodeline += "<li class=\"dropdown-item\"><a href=\"" + value["link"] + "\">" + value["notificationContent"] + "</a></li>";
    }
}
