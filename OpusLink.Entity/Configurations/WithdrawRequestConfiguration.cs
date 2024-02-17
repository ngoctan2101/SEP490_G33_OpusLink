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
    public class WithdrawRequestConfiguration : IEntityTypeConfiguration<WithdrawRequest>
    {
        public void Configure(EntityTypeBuilder<WithdrawRequest> builder)
        {
            builder.ToTable("WithdrawRequest");
            builder.HasKey(x => x.WithdrawRequestID);
            builder.Property(x => x.WithdrawRequestID).ValueGeneratedOnAdd();
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.Status).IsRequired().HasMaxLength(256);
            builder.HasOne(x => x.User).WithMany(x => x.WithdrawRequests).HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
