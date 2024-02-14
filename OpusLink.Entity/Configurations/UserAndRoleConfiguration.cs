using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Configurations
{
    public class UserAndRoleConfiguration : IEntityTypeConfiguration<UserAndRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserAndRole> builder)
        {
            builder.ToTable("UserAndRole");
            builder.HasData(
                new IdentityUserRole<int> { RoleId=1,UserId=9},
                new IdentityUserRole<int> { RoleId=3,UserId=1},
                new IdentityUserRole<int> { RoleId=3,UserId=2},
                new IdentityUserRole<int> { RoleId=3,UserId=3},
                new IdentityUserRole<int> { RoleId=2,UserId=4},
                new IdentityUserRole<int> { RoleId=2,UserId=5},
                new IdentityUserRole<int> { RoleId=2,UserId=6},
                new IdentityUserRole<int> { RoleId=2,UserId=7},
                new IdentityUserRole<int> { RoleId=2,UserId=8}
            );
        }
    }
}
