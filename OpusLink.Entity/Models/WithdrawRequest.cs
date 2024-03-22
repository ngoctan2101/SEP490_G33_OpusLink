using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class WithdrawRequest
    {
        public int WithdrawRequestID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public virtual User? User { get; set; }
        public string? Reason { get; set; }
        public int? HistoryPaymentID { get; set; }
    }
}
