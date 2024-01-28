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
    public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
    {
        public void Configure(EntityTypeBuilder<Milestone> builder)
        {
            builder.ToTable("Milestone");
            builder.HasKey(x => x.MilestoneID);
            builder.Property(x => x.MilestoneID).ValueGeneratedOnAdd();
            builder.Property(x => x.JobID).IsRequired();
            builder.Property(x => x.MilestoneContent).IsRequired();
            builder.Property(x => x.FilePathFreelancerUpload).HasMaxLength(256);
            builder.Property(x => x.Deadline).IsRequired();
            builder.Property(x => x.AmountToPay).IsRequired();
            builder.HasOne(x => x.Job).WithMany(x => x.Milestones).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
