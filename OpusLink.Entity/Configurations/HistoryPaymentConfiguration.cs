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
    public class HistoryPaymentConfiguration : IEntityTypeConfiguration<HistoryPayment>
    {
        public void Configure(EntityTypeBuilder<HistoryPayment> builder)
        {
            builder.ToTable("HistoryPayment");
            builder.HasKey(x => x.PaymentID);
            builder.Property(x => x.PaymentID).ValueGeneratedOnAdd();
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.TransactionType).IsRequired().HasMaxLength(256);
            builder.Property(x => x.TransactionDate).IsRequired();
            builder.Property(x => x.TransactionCode).IsRequired().HasMaxLength(256);
            builder.HasOne(x => x.User).WithMany(x => x.HistoryPayments).HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
