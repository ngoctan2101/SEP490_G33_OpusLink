using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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
            builder.HasData(
                new Offer { OfferID=1,FreelancerID=5,JobID=1,DateOffer= DateTime.ParseExact("2023-01-30 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), ProposedCost=280000,ExpectedDays=7,SelfIntroduction= "Đã có kinh nghiệm 3 năm làm web, mobile app đa lĩnh vực trong và ngoài nước", EstimatedPlan= "Nếu bạn quan tâm đến chào giá này, hãy reply cho mình biết. Mình sẽ setup một buổi meeting trao đổi chi tiết về doanh nghiệp của bạn và gửi sitemap, kế hoạch chi tiết trong vòng không quá 2 giờ sau đó." },
                new Offer { OfferID=2,FreelancerID=8,JobID=1,DateOffer= DateTime.ParseExact("2023-01-30 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), ProposedCost=500000,ExpectedDays=3,SelfIntroduction= "Tôi là một lập trình viên có nhiều năm kinh nghiệm phát triển các loại website, đặc biệt là các trang web bán hàng, giáo dục, bất động sản và y tế. Trong suốt sự nghiệp của mình, tôi tạo ra những trang web chất lượng cao, mang tính sáng tạo và tối ưu hoá hiệu suất. Tôi tự hào về việc đã đóng góp vào việc xây dựng nền tảng kỹ thuật vững chắc để hỗ trợ các doanh nghiệp bán hàng và các tổ chức giáo dục trong việc tăng cường hiệu quả kinh doanh và phục vụ học tập.\r\n\r\nTrong quá trình làm việc, tôi đã tiếp xúc và thành thạo các công nghệ đa dạng như HTML, CSS, JavaScript, PHP, Python và nhiều framework phổ biến như Vuejs, Nuxtjs, Reactjs, Nextjs, Laravel, Django, Express, Nestjs. Sự am hiểu sâu sắc về các công nghệ này giúp tôi tạo ra những trải nghiệm người dùng tuyệt vời và tích hợp những tính năng đa dạng, như thanh toán an toàn, quản lý tài khoản, đánh giá sản phẩm và nhiều tính năng tùy chỉnh khác.", EstimatedPlan= "Kế hoạch thực hiện công việc:\r\n-Thu thập thông tin khách hàng cũng nhu nhu cầu thiết kế.\r\n-Phân tích, báo giá và tiến hành thương lượng chốt sản phẩm.\r\n-Hoàn thành sản phẩm trong tiến độ đã thương lượng, test sản phẩm và bàn giao đến khách hàng.\r\n-Tiến hành sửa chữa, fix lỗi trong quá trình dùng thử.\r\n-Nhận thanh toán và áp dụng chính sách hậu mãi cho khách hàng" },
                new Offer { OfferID = 3, FreelancerID = 4, JobID = 3, DateOffer = DateTime.ParseExact("2023-01-30 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), ProposedCost = 1000000, ExpectedDays = 7, SelfIntroduction = "Toi co 2 nam kinh nghiem lam DataBase", EstimatedPlan = "Meeting trao doi chi tiet ve Requirement, sau do lam Database" },
                new Offer { OfferID = 4, FreelancerID = 5, JobID = 2, DateOffer = DateTime.ParseExact("2023-02-02 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), ProposedCost = 700000, ExpectedDays = 7, SelfIntroduction = "", EstimatedPlan = "Làm ngay sau khi có đầy đủ nội dung và yêu cầu. Bảo hành và bảo trì." },
                new Offer { OfferID = 5, FreelancerID = 8, JobID = 3, DateOffer = DateTime.ParseExact("2023-02-02 10:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), ProposedCost = 800000, ExpectedDays = 4, SelfIntroduction = "Toi co 3 nam kinh nghiem lam DataBase cho cong ty cong nghe noi tieng", EstimatedPlan = "Thu thập thông tin về requirement, phân tích, báo giá & thương lượng, hoàn thành theo tiến độ đã vạch ra, test, hỗ trợ 1 tháng sau khi bàn giao" }
                );
        }
    }
}
