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
    public class JobAndCategoryConfiguration : IEntityTypeConfiguration<JobAndCategory>
    {
        public void Configure(EntityTypeBuilder<JobAndCategory> builder)
        {
            builder.ToTable("JobAndCategory");
            builder.HasKey(x => x.JobAndCategoryID);
            builder.Property(x => x.JobAndCategoryID).ValueGeneratedOnAdd();
            builder.Property(x => x.CategoryID).IsRequired();
            builder.Property(x => x.JobID).IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.JobAndCategories).HasForeignKey(x => x.CategoryID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Job).WithMany(x => x.JobAndCategories).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new JobAndCategory{ JobAndCategoryID = 1,CategoryID=1,JobID=1 },
                new JobAndCategory{ JobAndCategoryID = 2,CategoryID=2,JobID=1 },
                new JobAndCategory{ JobAndCategoryID = 3,CategoryID=3,JobID=1 },
                new JobAndCategory{ JobAndCategoryID = 4,CategoryID=4,JobID=2 },
                new JobAndCategory{ JobAndCategoryID = 5,CategoryID=1,JobID=3 },
                new JobAndCategory{ JobAndCategoryID = 6,CategoryID=5,JobID=3 }
                );
        }
    }
}
