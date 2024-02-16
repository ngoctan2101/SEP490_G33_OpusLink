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
new Category { CategoryID = 1, CategoryParentID = null, CategoryName = "Web Development" },
new Category { CategoryID = 2, CategoryParentID = 1, CategoryName = "HTML" },
new Category { CategoryID = 3, CategoryParentID = 2, CategoryName = "HTML Accessibility" },
new Category { CategoryID = 4, CategoryParentID = 2, CategoryName = "HTML Optimization" },
new Category { CategoryID = 5, CategoryParentID = 2, CategoryName = "HTML SEO Best Practices" },
new Category { CategoryID = 6, CategoryParentID = 2, CategoryName = "HTML5 Game Development" },
new Category { CategoryID = 7, CategoryParentID = 2, CategoryName = "HTML Documentation and Guides" },
new Category { CategoryID = 8, CategoryParentID = 2, CategoryName = "HTML Prototyping" },
new Category { CategoryID = 9, CategoryParentID = 2, CategoryName = "Responsive Web Design" },
new Category { CategoryID = 10, CategoryParentID = 2, CategoryName = "HTML Forms" },
new Category { CategoryID = 11, CategoryParentID = 2, CategoryName = "HTML Structure and Semantics" },
new Category { CategoryID = 12, CategoryParentID = 2, CategoryName = "HTML Embedding" },
new Category { CategoryID = 13, CategoryParentID = 1, CategoryName = "CSS" },
new Category { CategoryID = 14, CategoryParentID = 1, CategoryName = "JavaScript" },
new Category { CategoryID = 15, CategoryParentID = 1, CategoryName = "React" },
new Category { CategoryID = 16, CategoryParentID = 1, CategoryName = "Angular" },
new Category { CategoryID = 17, CategoryParentID = 1, CategoryName = "Vue.js" },
new Category { CategoryID = 18, CategoryParentID = null, CategoryName = "Mobile App Development" },
new Category { CategoryID = 19, CategoryParentID = 18, CategoryName = "iOS" },
new Category { CategoryID = 20, CategoryParentID = 18, CategoryName = "Android" },
new Category { CategoryID = 21, CategoryParentID = 20, CategoryName = "Java" },
new Category { CategoryID = 22, CategoryParentID = 20, CategoryName = "Kotlin" },
new Category { CategoryID = 23, CategoryParentID = null, CategoryName = "Software Development" },
new Category { CategoryID = 24, CategoryParentID = 23, CategoryName = "Winform" },
new Category { CategoryID = 25, CategoryParentID = 24, CategoryName = "WinForms GUI Design" },
new Category { CategoryID = 26, CategoryParentID = 24, CategoryName = "Form Management and Navigation" },
new Category { CategoryID = 27, CategoryParentID = 24, CategoryName = "Error Handling and Logging" },
new Category { CategoryID = 28, CategoryParentID = 24, CategoryName = "WinForms Accessibility" },
new Category { CategoryID = 29, CategoryParentID = 23, CategoryName = "WPF" },
new Category { CategoryID = 30, CategoryParentID = null, CategoryName = "Database Development and Management" },
new Category { CategoryID = 31, CategoryParentID = 30, CategoryName = "SQL" },
new Category { CategoryID = 32, CategoryParentID = 31, CategoryName = "SQL Query Optimization" },
new Category { CategoryID = 33, CategoryParentID = 31, CategoryName = "SQL Backup and Recovery" },
new Category { CategoryID = 34, CategoryParentID = 31, CategoryName = "SQL Data Migration and Synchronization" },
new Category { CategoryID = 35, CategoryParentID = 31, CategoryName = "SQL Performance Testing" },
new Category { CategoryID = 36, CategoryParentID = 31, CategoryName = "SQL Data Security" },
new Category { CategoryID = 37, CategoryParentID = 31, CategoryName = "Database Migration and Upgrade" },
new Category { CategoryID = 38, CategoryParentID = 31, CategoryName = "Database Design and Modeling" },
new Category { CategoryID = 39, CategoryParentID = 30, CategoryName = "MySQL" },
new Category { CategoryID = 40, CategoryParentID = 30, CategoryName = "MongoDB" },
new Category { CategoryID = 41, CategoryParentID = 30, CategoryName = "PostgreSQL" },
new Category { CategoryID = 42, CategoryParentID = null, CategoryName = "Frontend Development" },
new Category { CategoryID = 43, CategoryParentID = 42, CategoryName = "UI" },
new Category { CategoryID = 44, CategoryParentID = 42, CategoryName = "UX" },
new Category { CategoryID = 45, CategoryParentID = null, CategoryName = "Backend Development" },
new Category { CategoryID = 46, CategoryParentID = null, CategoryName = "Full-Stack Development" },
new Category { CategoryID = 47, CategoryParentID = null, CategoryName = "DevOps" },
new Category { CategoryID = 48, CategoryParentID = null, CategoryName = "Cloud Computing" },
new Category { CategoryID = 49, CategoryParentID = 48, CategoryName = "AWS" },
new Category { CategoryID = 50, CategoryParentID = 48, CategoryName = "Azure" },
new Category { CategoryID = 51, CategoryParentID = 48, CategoryName = "Google Cloud" },
new Category { CategoryID = 52, CategoryParentID = null, CategoryName = "Cybersecurity" },
new Category { CategoryID = 53, CategoryParentID = null, CategoryName = "QA" },
new Category { CategoryID = 54, CategoryParentID = null, CategoryName = "Testing" },
new Category { CategoryID = 55, CategoryParentID = null, CategoryName = "IT Support and Helpdesk" },
new Category { CategoryID = 56, CategoryParentID = null, CategoryName = "Network Administration" },
new Category { CategoryID = 57, CategoryParentID = null, CategoryName = "Machine Learning and Artificial Intelligence" },
new Category { CategoryID = 58, CategoryParentID = null, CategoryName = "IoT (Internet of Things) Development" },
new Category { CategoryID = 59, CategoryParentID = null, CategoryName = "IT Consulting" }

                );
        }
    }
}
