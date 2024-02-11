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
                new Skill { SkillID=1,SkillParentID=null,SkillName="Web development"},
                new Skill { SkillID=2,SkillParentID=1,SkillName="code React"},
                new Skill { SkillID=3,SkillParentID=1,SkillName="code .net razor page"},
                new Skill { SkillID=4,SkillParentID=null,SkillName="design 2D"},
                new Skill { SkillID=5,SkillParentID=null,SkillName="communication"},
                new Skill { SkillID=6,SkillParentID=null,SkillName="English"},
                new Skill { SkillID=7,SkillParentID=null,SkillName="Teaching"}
                );
        }
    }
}
