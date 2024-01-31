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
    public class BlockWordRegExConfiguration : IEntityTypeConfiguration<BlockWordRegEx>
    {
        public void Configure(EntityTypeBuilder<BlockWordRegEx> builder)
        {
            builder.ToTable("BlockWordRegEx");
            builder.HasKey(x => x.PatternID);
            builder.Property(x => x.PatternID).ValueGeneratedOnAdd();
            builder.Property(x => x.Pattern).IsRequired().HasMaxLength(256);

        }
    }
}
