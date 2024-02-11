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
    public class FeedbackUserConfiguration : IEntityTypeConfiguration<FeedbackUser>
    {
        public void Configure(EntityTypeBuilder<FeedbackUser> builder)
        {
            builder.ToTable("FeedbackUser");
            builder.HasKey(x => x.FeedbackUserID);
            builder.Property(x => x.FeedbackUserID).ValueGeneratedOnAdd();
            builder.Property(x => x.CreateByUserID).IsRequired();
            builder.Property(x => x.TargetToUserID).IsRequired();
            builder.Property(x => x.Star).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.HasOne(x => x.CreateByUser).WithMany(x => x.FeedbackUsersAsACreater).HasForeignKey(x => x.CreateByUserID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.TargetToUser).WithMany(x => x.FeedbackUsersAsATargeter).HasForeignKey(x => x.TargetToUserID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
