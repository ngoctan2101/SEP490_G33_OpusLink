using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpusLink.Entity.Models;

namespace OpusLink.Entity.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");
            builder.HasKey(x => x.LocationID);
            builder.Property(x => x.LocationID).ValueGeneratedOnAdd();
            builder.Property(x => x.LocationName).IsRequired().HasMaxLength(256);
            builder.HasData(
                new Location { LocationID = 1, LocationName = "Hà Giang" },
new Location { LocationID = 2, LocationName = "Cao Bằng" },
new Location { LocationID = 3, LocationName = "Lào Cai" },
new Location { LocationID = 4, LocationName = "Sơn La" },
new Location { LocationID = 5, LocationName = "Lai Châu" },
new Location { LocationID = 6, LocationName = "Bắc Kạn" },
new Location { LocationID = 7, LocationName = "Lạng Sơn" },
new Location { LocationID = 8, LocationName = "Tuyên Quang" },
new Location { LocationID = 9, LocationName = "Yên Bái" },
new Location { LocationID = 10, LocationName = "Thái Nguyên" },
new Location { LocationID = 11, LocationName = "Điện Biên" },
new Location { LocationID = 12, LocationName = "Phú Thọ" },
new Location { LocationID = 13, LocationName = "Vĩnh Phúc" },
new Location { LocationID = 14, LocationName = "Bắc Giang" },
new Location { LocationID = 15, LocationName = "Bắc Ninh" },
new Location { LocationID = 16, LocationName = "Hà Nội" },
new Location { LocationID = 17, LocationName = "Quảng Ninh" },
new Location { LocationID = 18, LocationName = "Hải Dương" },
new Location { LocationID = 19, LocationName = "Hải Phòng" },
new Location { LocationID = 20, LocationName = "Hòa Bình" },
new Location { LocationID = 21, LocationName = "Hưng Yên" }

                );
        }
    }
}
