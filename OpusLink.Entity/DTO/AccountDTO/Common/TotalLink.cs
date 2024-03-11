using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO.Common
{
    public class TotalLink
    {
        public static string LinkValidRegister = "https://localhost:7131/Account/EmailVerify";
        public static string LinkForgotPassword = "https://localhost:7131/Account/ResetPassword";
    }
}
