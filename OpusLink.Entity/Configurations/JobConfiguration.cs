using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using OpusLink.Shared.Enums;

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
            //builder.Property(x => x.Status).HasMaxLength(20);
            builder.Property(x => x.EmployerID).IsRequired();
            builder.Property(x => x.Status).HasColumnType("smallint");
            builder.HasOne(x => x.Employer).WithMany(x => x.JobsAsAnEmployer).HasForeignKey(x => x.EmployerID);
            builder.HasOne(x => x.Freelancer).WithMany(x => x.JobsAsAFreelancer).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Location).WithMany(x => x.Jobs).HasForeignKey(x => x.LocationID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new Job {JobID=1, EmployerID=1,FreelancerID=5, JobTitle="Tim DEV", JobContent="Minh can 1 nguoi code web", BudgetFrom=300000,BudgetTo=500000, DateCreated = DateTime.ParseExact("2023-01-29 21:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),LocationID=1,Status= (int)JobStatusEnum.Hired },
                new Job {JobID=2, EmployerID=2,FreelancerID=null, JobTitle="Tim BA làm requirement", JobContent="Can 1 nguoi hieu ve nghiep vu ngan hang để tạo ra tài liệu requirement cho trang web", BudgetFrom = 200000, BudgetTo = 800000, DateCreated = DateTime.ParseExact("2023-01-29 21:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), LocationID = 1, Status = (int)JobStatusEnum.Approved },
                new Job {JobID=3, EmployerID=2,FreelancerID=8, JobTitle="Thiet ke Database",JobContent="Can nguoi giup minh thiet ke DataBase cho trang web giao duc", BudgetFrom = 400000, BudgetTo = 1000000, DateCreated = DateTime.ParseExact("2023-01-29 22:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), LocationID = 2, Status = (int)JobStatusEnum.Hired }
                );
        }
    }
}
