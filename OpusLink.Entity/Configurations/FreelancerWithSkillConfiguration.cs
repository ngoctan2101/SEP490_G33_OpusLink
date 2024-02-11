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
    public class FreelancerWithSkillConfiguration : IEntityTypeConfiguration<FreelancerWithSkill>
    {
        public void Configure(EntityTypeBuilder<FreelancerWithSkill> builder)
        {
            builder.ToTable("FreelancerWithSkill");
            builder.HasKey(x => x.FreelancerWithSkillID);
            builder.Property(x => x.FreelancerWithSkillID).ValueGeneratedOnAdd();
            builder.Property(x => x.SkillID).IsRequired();
            builder.Property(x => x.FreelancerID).IsRequired();
            builder.HasOne(x => x.Freelancer).WithMany(x => x.FreelancerWithSkills).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Skill).WithMany(x => x.FreelancerWithSkills).HasForeignKey(x => x.SkillID).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(
                new FreelancerWithSkill{ FreelancerWithSkillID=1, FreelancerID=4,SkillID=1 },
                new FreelancerWithSkill{ FreelancerWithSkillID=2, FreelancerID=5,SkillID=2 },
                new FreelancerWithSkill{ FreelancerWithSkillID=3, FreelancerID=6,SkillID=3 },
                new FreelancerWithSkill{ FreelancerWithSkillID=4, FreelancerID=7,SkillID=4 },
                new FreelancerWithSkill{ FreelancerWithSkillID=5, FreelancerID=8,SkillID=5 },
                new FreelancerWithSkill{ FreelancerWithSkillID=6, FreelancerID=4,SkillID=6 },
                new FreelancerWithSkill{ FreelancerWithSkillID=7, FreelancerID=7,SkillID=7 },
                new FreelancerWithSkill{ FreelancerWithSkillID=8, FreelancerID=5,SkillID=1 },
                new FreelancerWithSkill{ FreelancerWithSkillID=9, FreelancerID=6,SkillID=2 },
                new FreelancerWithSkill{ FreelancerWithSkillID=10, FreelancerID=7,SkillID=3 },
                new FreelancerWithSkill{ FreelancerWithSkillID=11, FreelancerID=8,SkillID=4 },
                new FreelancerWithSkill{ FreelancerWithSkillID=12, FreelancerID=7,SkillID=5 }
                );
        }
    }
}
