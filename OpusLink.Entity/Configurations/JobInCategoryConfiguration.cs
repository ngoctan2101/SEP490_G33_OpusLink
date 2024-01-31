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
    public class JobInCategoryConfiguration : IEntityTypeConfiguration<JobInCategory>
    {
        public void Configure(EntityTypeBuilder<JobInCategory> builder)
        {
            builder.ToTable("JobInCategory");
            builder.HasKey(x => x.JobInCategoryID);
            builder.Property(x => x.JobInCategoryID).ValueGeneratedOnAdd();
            builder.Property(x => x.CategoryID).IsRequired();
            builder.Property(x => x.JobID).IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.JobInCategories).HasForeignKey(x => x.CategoryID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Job).WithMany(x => x.JobInCategories).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new JobInCategory{JobInCategoryID=1,CategoryID=1,JobID=1 },
                new JobInCategory{JobInCategoryID=2,CategoryID=2,JobID=1 },
                new JobInCategory{JobInCategoryID=3,CategoryID=3,JobID=1 },
                new JobInCategory{JobInCategoryID=4,CategoryID=4,JobID=2 },
                new JobInCategory{JobInCategoryID=5,CategoryID=1,JobID=3 },
                new JobInCategory{JobInCategoryID=6,CategoryID=5,JobID=3 }
                );
        }
    }
}
