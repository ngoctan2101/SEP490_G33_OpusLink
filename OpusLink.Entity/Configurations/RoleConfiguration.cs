using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasData(
                new IdentityRole<int> { Id=1,Name="Admin", NormalizedName ="ADMIN"},
                new IdentityRole<int> { Id=2,Name= "Freelancer", NormalizedName = "FREELANCER" },
                new IdentityRole<int> { Id=3,Name= "Employer", NormalizedName = "EMPLOYER" }
            ) ;
        }
    }
}
