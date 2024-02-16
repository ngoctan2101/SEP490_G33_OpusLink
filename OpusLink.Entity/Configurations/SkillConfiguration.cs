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
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skill");
            builder.HasKey(x => x.SkillID);
            builder.Property(x => x.SkillID).ValueGeneratedOnAdd();
            builder.Property(x => x.SkillName).IsRequired().HasMaxLength(256);
            builder.HasOne(x => x.SkillParent).WithMany().HasForeignKey(x => x.SkillParentID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
new Skill { SkillID = 1, SkillParentID = null, SkillName = "Problem-Solving Skills" },
new Skill { SkillID = 2, SkillParentID = null, SkillName = "SEO (Search Engine Optimization)" },
new Skill { SkillID = 3, SkillParentID = null, SkillName = "Client Management" },
new Skill { SkillID = 4, SkillParentID = null, SkillName = "Time Management" },
new Skill { SkillID = 5, SkillParentID = null, SkillName = "Collaboration" },
new Skill { SkillID = 6, SkillParentID = null, SkillName = "Negotiation Skills" },
new Skill { SkillID = 7, SkillParentID = null, SkillName = "Leadership" },
new Skill { SkillID = 8, SkillParentID = null, SkillName = "Presentation Skills" },
new Skill { SkillID = 9, SkillParentID = null, SkillName = "Networking" },
new Skill { SkillID = 10, SkillParentID = null, SkillName = "Adaptability" },
new Skill { SkillID = 11, SkillParentID = null, SkillName = "Programming Languages" },
new Skill { SkillID = 12, SkillParentID = 11, SkillName = "Python" },
new Skill { SkillID = 13, SkillParentID = 11, SkillName = "Java" },
new Skill { SkillID = 14, SkillParentID = 11, SkillName = "C++" },
new Skill { SkillID = 15, SkillParentID = 11, SkillName = "JavaScript" },
new Skill { SkillID = 16, SkillParentID = 11, SkillName = "Ruby" },
new Skill { SkillID = 17, SkillParentID = null, SkillName = "Web Development" },
new Skill { SkillID = 18, SkillParentID = 17, SkillName = "HTML" },
new Skill { SkillID = 19, SkillParentID = 17, SkillName = "CSS" },
new Skill { SkillID = 20, SkillParentID = 17, SkillName = "JavaScript " },
new Skill { SkillID = 21, SkillParentID = null, SkillName = "Mobile App Development" },
new Skill { SkillID = 22, SkillParentID = 21, SkillName = "Swift" },
new Skill { SkillID = 23, SkillParentID = 21, SkillName = "Kotlin/Java" },
new Skill { SkillID = 24, SkillParentID = null, SkillName = "Database Management" },
new Skill { SkillID = 25, SkillParentID = 24, SkillName = "PostgreSQL" },
new Skill { SkillID = 26, SkillParentID = 24, SkillName = "MySQL" },
new Skill { SkillID = 27, SkillParentID = 24, SkillName = "MongoDB" },
new Skill { SkillID = 28, SkillParentID = 24, SkillName = "SQLite " },
new Skill { SkillID = 29, SkillParentID = null, SkillName = "Version Control" },
new Skill { SkillID = 30, SkillParentID = 29, SkillName = "Git " },
new Skill { SkillID = 31, SkillParentID = null, SkillName = "Frontend Frameworks" },
new Skill { SkillID = 32, SkillParentID = 31, SkillName = "React.js" },
new Skill { SkillID = 33, SkillParentID = 31, SkillName = "Angular" },
new Skill { SkillID = 34, SkillParentID = 31, SkillName = "Vue.js" },
new Skill { SkillID = 35, SkillParentID = null, SkillName = "Backend Frameworks" },
new Skill { SkillID = 36, SkillParentID = 35, SkillName = "Node.js" },
new Skill { SkillID = 37, SkillParentID = 35, SkillName = "Django" },
new Skill { SkillID = 38, SkillParentID = 35, SkillName = "Flask" },
new Skill { SkillID = 39, SkillParentID = 35, SkillName = "Spring Boot" },
new Skill { SkillID = 40, SkillParentID = null, SkillName = "API Integration" },
new Skill { SkillID = 41, SkillParentID = null, SkillName = "Cloud Computing" },
new Skill { SkillID = 42, SkillParentID = 41, SkillName = "AWS " },
new Skill { SkillID = 43, SkillParentID = 41, SkillName = "Azure" },
new Skill { SkillID = 44, SkillParentID = 41, SkillName = "Google Cloud" },
new Skill { SkillID = 45, SkillParentID = null, SkillName = "DevOps Practices" },
new Skill { SkillID = 46, SkillParentID = 45, SkillName = "Docker" },
new Skill { SkillID = 47, SkillParentID = 45, SkillName = "Kubernetes" },
new Skill { SkillID = 48, SkillParentID = 45, SkillName = "Jenkins" },
new Skill { SkillID = 49, SkillParentID = 45, SkillName = "Travis CI" },
new Skill { SkillID = 50, SkillParentID = null, SkillName = "Cybersecurity" },
new Skill { SkillID = 51, SkillParentID = null, SkillName = "Agile Methodologies" },
new Skill { SkillID = 52, SkillParentID = 51, SkillName = "Scrum " },
new Skill { SkillID = 53, SkillParentID = 51, SkillName = "Kanban " },
new Skill { SkillID = 54, SkillParentID = null, SkillName = "Software Testing" },
new Skill { SkillID = 55, SkillParentID = 54, SkillName = "unit testing" },
new Skill { SkillID = 56, SkillParentID = 54, SkillName = "integration testing" },
new Skill { SkillID = 57, SkillParentID = 54, SkillName = "end-to-end testing" },
new Skill { SkillID = 58, SkillParentID = null, SkillName = "UI/UX Design Principles" },
new Skill { SkillID = 59, SkillParentID = null, SkillName = "Machine Learning/Artificial Intelligence" },
new Skill { SkillID = 60, SkillParentID = null, SkillName = "Microservices Architecture" },
new Skill { SkillID = 61, SkillParentID = null, SkillName = "Linux/Unix Systems Administration" },
new Skill { SkillID = 62, SkillParentID = null, SkillName = "Web Security" }

                );
        }
    }
}
