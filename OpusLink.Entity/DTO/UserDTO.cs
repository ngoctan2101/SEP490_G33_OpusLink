using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? FullNameOnIDCard { get; set; }
        public string? IDNumber { get; set; }
        public DateTime? Dob { get; set; }
        public decimal? StarMedium { get; set; }
        public string? Introduction { get; set; }
        public string? CVFilePath { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountInfor { get; set; }
        public bool IsVeryfiedIdentity { get; set; }
        public decimal? AmountMoney { get; set; }
        public int Status { get; set; }

        public List<String> Skills { get; set; }
    }
}
