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
    public class ChatBoxConfiguration : IEntityTypeConfiguration<ChatBox>
    {
        public void Configure(EntityTypeBuilder<ChatBox> builder)
        {
            builder.ToTable("ChatBox");
            builder.HasKey(x => x.ChatBoxID);
            builder.Property(x => x.ChatBoxID).ValueGeneratedOnAdd();
            builder.Property(x => x.IsViewed).IsRequired();
            builder.HasOne(x => x.Employer).WithMany(x => x.ChatBoxsAsAnEmployer).HasForeignKey(x => x.EmployerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Freelancer).WithMany(x => x.ChatBoxsAsAFreelancer).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict); ;
        }
    }
}
