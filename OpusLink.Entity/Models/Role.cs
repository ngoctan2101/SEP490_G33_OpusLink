using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Role : IdentityRole<Int32>
    {
        private string Name { get; set; }
        public Role(string name) : base(name)
        {
            Name = name;
        }
    }
}
