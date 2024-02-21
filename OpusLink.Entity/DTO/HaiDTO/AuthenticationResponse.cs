using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.HaiDTO
{
    public class AuthenticationResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public long ExpiredTime { get; set; } //Còn bao nhiêu thời gian của JWT
        public string Role { get; set; }
        public string UserId { get; set; }
    }
}
