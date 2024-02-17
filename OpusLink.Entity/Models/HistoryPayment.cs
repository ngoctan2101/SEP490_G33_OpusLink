using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class HistoryPayment
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionCode { get; set; }

        public virtual User User { get; set; }
    }
}
