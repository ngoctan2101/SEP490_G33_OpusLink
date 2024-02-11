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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.CategoryID);
            builder.Property(x => x.CategoryID).ValueGeneratedOnAdd();
            builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(256);
            builder.HasOne(x => x.CategoryParent).WithMany().HasForeignKey(x => x.CategoryParentID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new Category {CategoryID=1,CategoryParentID=null,CategoryName="Web dev" },
                new Category {CategoryID=2,CategoryParentID=1,CategoryName="BE dev" },
                new Category {CategoryID=3,CategoryParentID=1,CategoryName="FE dev" },
                new Category {CategoryID=4,CategoryParentID=null,CategoryName="BA" },
                new Category {CategoryID=5,CategoryParentID=null,CategoryName="Dạy học trực tuyến" }
                );
        }
    }
}
