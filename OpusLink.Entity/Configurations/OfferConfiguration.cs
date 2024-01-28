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
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.ToTable("Offer");
            builder.HasKey(x => x.OfferID);
            builder.Property(x => x.OfferID).ValueGeneratedOnAdd();
            builder.Property(x => x.FreelancerID).IsRequired();
            builder.Property(x => x.JobID).IsRequired();
            builder.Property(x => x.ExpectedDays).HasColumnType("smallint");
            builder.HasOne(x => x.Freelancer).WithMany(x => x.OffersAsAFreelancer).HasForeignKey(x => x.FreelancerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Job).WithMany(x => x.Offers).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
