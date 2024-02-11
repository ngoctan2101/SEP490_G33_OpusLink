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
    public class ReportUserConfiguration : IEntityTypeConfiguration<ReportUser>
    {
        public void Configure(EntityTypeBuilder<ReportUser> builder)
        {
            builder.ToTable("ReportUser");
            builder.HasKey(x => x.ReportUserID);
            builder.Property(x => x.ReportUserID).ValueGeneratedOnAdd();
            builder.Property(x => x.CreateByUserID).IsRequired();
            builder.Property(x => x.TargetToUserID).IsRequired();
            builder.Property(x => x.ReportUserContent).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.HasOne(x => x.CreateByUser).WithMany(x => x.ReportUsersAsACreater).HasForeignKey(x => x.CreateByUserID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.TargetToUser).WithMany(x => x.ReportUsersAsATargeter).HasForeignKey(x => x.TargetToUserID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
