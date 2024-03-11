using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO
{
    public class ReSendEmailDTO
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
