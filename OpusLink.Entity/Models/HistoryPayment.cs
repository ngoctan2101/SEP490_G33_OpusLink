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

        //Deposit=1, tiền gửi
        //Withdraw =2, Rút tiền
        //PutToEscrow =3, Đặt cọc tiền
        //FeePostJob = 4, Tiền đăng bài
        //EarnAMilestone=5, Tiền nhận ở mỗi miston
        //HoldedAJob = 6, Tiền giữ 20%  
    }
}
