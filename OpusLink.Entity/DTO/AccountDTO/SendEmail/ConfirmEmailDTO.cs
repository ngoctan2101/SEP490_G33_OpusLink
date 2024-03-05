using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO.SendEmail
{
    public class ConfirmEmailDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
