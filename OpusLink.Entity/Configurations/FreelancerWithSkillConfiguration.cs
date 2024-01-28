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
        }
    }
}
