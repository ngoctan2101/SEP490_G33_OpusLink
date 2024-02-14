using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusLink.Shared.Enums;

namespace OpusLink.Entity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(128);
            builder.Property(x => x.ProfilePicture).HasMaxLength(256);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.Address).HasMaxLength(256);
            builder.Property(x => x.FullNameOnIDCard).HasMaxLength(256);
            builder.Property(x => x.IDNumber).HasMaxLength(128);
            builder.Property(x => x.CVFilePath).HasMaxLength(256);
            builder.Property(x => x.BankName).HasMaxLength(256);
            builder.Property(x => x.IsVeryfiedIdentity).IsRequired();
            builder.Property(x => x.Status).HasColumnType("smallint");
            builder.HasData(
                new User { Id =1,UserName="Nguyen Van A", Email="nva123@gmail.com",PasswordHash="test",Status=(int)UserStatusEnum.NotBanned,IsVeryfiedIdentity=false, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=1},
                new User { Id =2,UserName="Nguyen Van B", Email="nvb123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=true, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=2},
                new User { Id =3,UserName="Nguyen Van C", Email="nvc123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=false, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=0},
                new User { Id =4,UserName="Tran Van D", Email="tvd123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=3},
                new User { Id =5,UserName="Tran Thi E", Email="tte123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=4},
                new User { Id =6,UserName="Tran Van F", Email="tvf123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=false, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=5},
                new User { Id =7,UserName="Tran Thi G", Email="ttg123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=true, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=6},
                new User { Id =8,UserName="Tran Thi H", Email="tth123@gmail.com",PasswordHash="test", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=7},
                new User { Id =9,UserName="admin", Email= "admin@gmail.com", PasswordHash="admin", Status = (int)UserStatusEnum.NotBanned, IsVeryfiedIdentity=false, EmailConfirmed=true, PhoneNumberConfirmed=false, TwoFactorEnabled=false, LockoutEnabled=false, AccessFailedCount=7}
                
                );
        }
    }
}
