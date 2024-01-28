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
    public class SaveJobConfiguration : IEntityTypeConfiguration<SaveJob>
    {
        public void Configure(EntityTypeBuilder<SaveJob> builder)
        {
            builder.ToTable("SaveJob");
            builder.HasKey(x => x.SaveJobID);
            builder.Property(x => x.SaveJobID).ValueGeneratedOnAdd();
            builder.Property(x => x.JobID).IsRequired();
            builder.Property(x => x.FreelancerID).IsRequired();
            builder.HasOne(x => x.Job).WithMany(x => x.SaveJobs).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Freelancer).WithMany(x => x.SaveJobs).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
