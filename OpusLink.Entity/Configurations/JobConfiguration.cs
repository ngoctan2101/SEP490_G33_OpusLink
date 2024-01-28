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
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Job");
            builder.HasKey(x => x.JobID);
            builder.Property(x => x.JobID).ValueGeneratedOnAdd();
            builder.Property(x => x.JobTitle).HasMaxLength(256);
            builder.Property(x => x.Status).HasMaxLength(20);
            builder.Property(x => x.EmployerID).IsRequired();
            builder.HasOne(x => x.Employer).WithMany(x => x.JobsAsAnEmployer).HasForeignKey(x => x.EmployerID);
            builder.HasOne(x => x.Freelancer).WithMany(x => x.JobsAsAFreelancer).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Location).WithMany(x => x.Jobs).HasForeignKey(x => x.LocationID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
