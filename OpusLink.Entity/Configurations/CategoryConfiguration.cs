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
        }
    }
}
