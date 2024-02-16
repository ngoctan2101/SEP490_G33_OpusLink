using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpusLink.Entity.Configurations;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusLink.Shared.Enums;

namespace OpusLink.Entity
{
    public class OpusLinkDBContext : IdentityDbContext<User, Role, int,IdentityUserClaim<int>, UserAndRole
                                                        , IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public OpusLinkDBContext()
        {

        }
        public OpusLinkDBContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("OpusLink"));
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserAndRoleConfiguration());
            builder.ApplyConfiguration(new BlockWordRegExConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ChatBoxConfiguration());
            builder.ApplyConfiguration(new FeedbackUserConfiguration());

            builder.ApplyConfiguration(new HistoryPaymentConfiguration());
            builder.ApplyConfiguration(new JobConfiguration());
            builder.ApplyConfiguration(new JobAndCategoryConfiguration());
            builder.ApplyConfiguration(new LocationConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new MilestoneConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new OfferConfiguration());
            builder.ApplyConfiguration(new ReportJobConfiguration());
            builder.ApplyConfiguration(new ReportUserConfiguration());
            builder.ApplyConfiguration(new SaveJobConfiguration());
            builder.ApplyConfiguration(new SearchJobFormConfiguration());
            builder.ApplyConfiguration(new SkillConfiguration());
            builder.ApplyConfiguration(new FreelancerAndSkillConfiguration());
            builder.ApplyConfiguration(new WithdrawRequestConfiguration());
            base.OnModelCreating(builder);
        }

        public virtual DbSet<BlockWordRegEx> BlockWordRegExes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ChatBox> ChatBoxs { get; set; } = null!;
        public virtual DbSet<FeedbackUser> FeedbackUsers { get; set; } = null!;
        public virtual DbSet<FreelancerAndSkill> FreelancerAndSkills { get; set; } = null!;
        public virtual DbSet<HistoryPayment> HistoryPayments { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobAndCategory> JobAndCategories { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UserAndRole> UserAndRoles { get; set; } = null!;

        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Milestone> Milestones { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Offer> Offers { get; set; } = null!;
        public virtual DbSet<ReportJob> ReportJobs { get; set; } = null!;
        public virtual DbSet<ReportUser> ReportUsers { get; set; } = null!;
        public virtual DbSet<SaveJob> SaveJobs { get; set; } = null!;
        public virtual DbSet<SearchJobForm> SearchJobForms { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WithdrawRequest> WithdrawRequests { get; set; } = null!;

    }
}
