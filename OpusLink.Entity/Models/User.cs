using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class User : IdentityUser<Int32>
    {
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public string? FullNameOnIDCard { get; set; }
        public string? IDNumber { get; set; }
        public DateTime? Dob { get; set; }
        public decimal? StarMedium { get; set; }
        public string? Introduction { get; set; }
        public string? CVFilePath { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountInfor { get; set; }
        public bool IsVeryfiedIdentity { get; set; }
        public decimal? AmountMoney { get; set; }
        public int Status { get; set; }
        public virtual ICollection<ChatBox> ChatBoxsAsAFreelancer { get; set; } = new List<ChatBox>();
        public virtual ICollection<ChatBox> ChatBoxsAsAnEmployer { get; set; } = new List<ChatBox>();
        public virtual ICollection<Offer> OffersAsAFreelancer { get; set; } = new List<Offer>();
        public virtual ICollection<Job> JobsAsAFreelancer { get; set; } = new List<Job>();
        public virtual ICollection<Job> JobsAsAnEmployer { get; set; } = new List<Job>();
        public virtual ICollection<ReportJob> ReportJobs { get; set; } = new List<ReportJob>();
        public virtual ICollection<SaveJob> SaveJobs { get; set; } = new List<SaveJob>();
        public virtual ICollection<ReportUser> ReportUsersAsACreater { get; set; } = new List<ReportUser>();
        public virtual ICollection<ReportUser> ReportUsersAsATargeter { get; set; } = new List<ReportUser>();
        public virtual ICollection<FeedbackUser> FeedbackUsersAsACreater { get; set; } = new List<FeedbackUser>();
        public virtual ICollection<FeedbackUser> FeedbackUsersAsATargeter { get; set; } = new List<FeedbackUser>();
        public virtual ICollection<WithdrawRequest> WithdrawRequests { get; set; } = new List<WithdrawRequest>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<HistoryPayment> HistoryPayments { get; set; } = new List<HistoryPayment>();
        public virtual ICollection<FreelancerAndSkill> FreelancerAndSkills { get; set; } = new List<FreelancerAndSkill>();
    }
}
