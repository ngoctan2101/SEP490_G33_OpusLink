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
        }
    }
}
