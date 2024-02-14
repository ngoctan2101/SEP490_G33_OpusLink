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
    public class FreelancerAndSkillConfiguration : IEntityTypeConfiguration<FreelancerAndSkill>
    {
        public void Configure(EntityTypeBuilder<FreelancerAndSkill> builder)
        {
            builder.ToTable("FreelancerAndSkill");
            builder.HasKey(x => x.FreelancerAndSkillID);
            builder.Property(x => x.FreelancerAndSkillID).ValueGeneratedOnAdd();
            builder.Property(x => x.SkillID).IsRequired();
            builder.Property(x => x.FreelancerID).IsRequired();
            builder.HasOne(x => x.Freelancer).WithMany(x => x.FreelancerAndSkills).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Skill).WithMany(x => x.FreelancerAndSkills).HasForeignKey(x => x.SkillID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new FreelancerAndSkill{ FreelancerAndSkillID = 1, FreelancerID=4,SkillID=1 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 2, FreelancerID=5,SkillID=2 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 3, FreelancerID=6,SkillID=3 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 4, FreelancerID=7,SkillID=4 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 5, FreelancerID=8,SkillID=5 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 6, FreelancerID=4,SkillID=6 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 7, FreelancerID=7,SkillID=7 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 8, FreelancerID=5,SkillID=1 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 9, FreelancerID=6,SkillID=2 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 10, FreelancerID=7,SkillID=3 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 11, FreelancerID=8,SkillID=4 },
                new FreelancerAndSkill{ FreelancerAndSkillID = 12, FreelancerID=7,SkillID=5 }
                );
        }
    }
}
