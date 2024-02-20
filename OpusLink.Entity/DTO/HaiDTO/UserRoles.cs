using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.HaiDTO
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string Freelancer = "Freelancer";
        public const string Employer = "Employer";
        public static readonly List<string> Roles = new List<string> { Admin, Freelancer, Employer };
    }
}
