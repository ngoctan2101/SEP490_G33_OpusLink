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
    public class ReportJobConfiguration : IEntityTypeConfiguration<ReportJob>
    {
        public void Configure(EntityTypeBuilder<ReportJob> builder)
        {
            builder.ToTable("ReportJob");
            builder.HasKey(x => x.ReportJobID);
            builder.Property(x => x.ReportJobID).ValueGeneratedOnAdd();
            builder.Property(x => x.TargetToJob).IsRequired();
            builder.Property(x => x.CreateByFreelancer).IsRequired();
            builder.Property(x => x.ReportJobContent).IsRequired().HasMaxLength(256);
            builder.Property(x => x.DateCreated).IsRequired();
            builder.HasOne(x => x.Job).WithMany(x => x.ReportJobs).HasForeignKey(x => x.TargetToJob).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Freelancer).WithMany(x => x.ReportJobs).HasForeignKey(x => x.TargetToJob).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
