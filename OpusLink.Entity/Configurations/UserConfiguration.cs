using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(128);
            builder.Property(x => x.ProfilePicture).HasMaxLength(256);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.Address).HasMaxLength(256);
            builder.Property(x => x.FullNameOnIDCard).HasMaxLength(256);
            builder.Property(x => x.IDNumber).HasMaxLength(128);
            builder.Property(x => x.CVFilePath).HasMaxLength(256);
            builder.Property(x => x.BankName).HasMaxLength(256);
            builder.Property(x => x.IsVeryfiedIdentity).IsRequired();
        }
    }
}
