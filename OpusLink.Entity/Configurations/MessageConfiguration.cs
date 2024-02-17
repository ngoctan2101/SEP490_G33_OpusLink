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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");
            builder.HasKey(x => x.MessageID);
            builder.Property(x => x.MessageID).ValueGeneratedOnAdd();
            builder.Property(x => x.FromEmployer).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.MessageContent).IsRequired().HasMaxLength(256);
            builder.HasOne(x => x.ChatBox).WithMany(x => x.Messages).HasForeignKey(x => x.ChatBoxID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
