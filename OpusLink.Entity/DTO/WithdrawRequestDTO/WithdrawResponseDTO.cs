using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.WithdrawRequestDTO
{
    public class WithdrawResponseDTO
    {
        public int WithdrawRequestID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public string? Reason { get; set; }
        public int? HistoryPaymentID { get; set; }
        public string? UserName { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountInfor { get; set; }
        public decimal AmountUser { get; set; }
    }
}
