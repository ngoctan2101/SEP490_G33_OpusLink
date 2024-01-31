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
    public class SearchJobFormConfiguration : IEntityTypeConfiguration<SearchJobForm>
    {
        public void Configure(EntityTypeBuilder<SearchJobForm> builder)
        {
            builder.ToTable("SearchJobForm");
            builder.HasKey(x => x.FormID);
            builder.Property(x => x.FormID).ValueGeneratedOnAdd();
            builder.Property(x => x.CategoryID).IsRequired();
            builder.Property(x => x.LocationID).IsRequired();
            builder.HasData(
                new SearchJobForm{FormID=1,CategoryID=1,LocationID=1 },
                new SearchJobForm{FormID=2,CategoryID=2,LocationID=2 },
                new SearchJobForm{FormID=3,CategoryID=5,LocationID=2 }
                );
        }
    }
}
