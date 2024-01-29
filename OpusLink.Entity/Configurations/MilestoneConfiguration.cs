using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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
            builder.HasData(
                new Milestone { MilestoneID=1,JobID=1,MilestoneContent="Moc 1:...",FilePathFreelancerUpload= "Job1Moc1.pdf", Deadline= DateTime.ParseExact("2024-02-10 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),  AmountToPay=100000},
                new Milestone { MilestoneID=2,JobID=1,MilestoneContent="Moc 2:...",FilePathFreelancerUpload= "Job1Moc2.pdf", Deadline= DateTime.ParseExact("2024-02-11 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),  AmountToPay=300000},
                new Milestone { MilestoneID=3,JobID=3,MilestoneContent= "Moc 1:...", FilePathFreelancerUpload= "Job3Moc1.pdf", Deadline= DateTime.ParseExact("2024-02-11 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),  AmountToPay=100000},
                new Milestone { MilestoneID=4,JobID=3,MilestoneContent= "Moc 2:...", FilePathFreelancerUpload= "Job3Moc2.pdf", Deadline= DateTime.ParseExact("2024-02-12 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),  AmountToPay=200000},
                new Milestone { MilestoneID=5,JobID=3,MilestoneContent= "Moc 3:...", FilePathFreelancerUpload= "Job3Moc3.pdf", Deadline= DateTime.ParseExact("2024-02-13 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),  AmountToPay=300000}
                );
        }
    }
}
