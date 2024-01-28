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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(x => x.NotificationID);
            builder.Property(x => x.NotificationID).ValueGeneratedOnAdd();
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.NotificationContent).IsRequired();
            builder.Property(x => x.IsReaded).IsRequired();
            builder.Property(x => x.Link).IsRequired().HasMaxLength(256);
            builder.Property(x => x.NotificationDate).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
