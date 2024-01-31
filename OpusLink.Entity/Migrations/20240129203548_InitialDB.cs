using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpusLink.Entity.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FullNameOnIDCard = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IDNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StarMedium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVFilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BankAccountInfor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVeryfiedIdentity = table.Column<bool>(type: "bit", nullable: false),
                    AmountMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "BlockWordRegEx",
                columns: table => new
                {
                    PatternID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pattern = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockWordRegEx", x => x.PatternID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryParentID = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_Category_CategoryParentID",
                        column: x => x.CategoryParentID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "SearchJobForm",
                columns: table => new
                {
                    FormID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchJobForm", x => x.FormID);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    SkillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillParentID = table.Column<int>(type: "int", nullable: true),
                    SkillName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.SkillID);
                    table.ForeignKey(
                        name: "FK_Skill_Skill_SkillParentID",
                        column: x => x.SkillParentID,
                        principalTable: "Skill",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatBox",
                columns: table => new
                {
                    ChatBoxID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployerID = table.Column<int>(type: "int", nullable: false),
                    FreelancerID = table.Column<int>(type: "int", nullable: false),
                    IsViewed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatBox", x => x.ChatBoxID);
                    table.ForeignKey(
                        name: "FK_ChatBox_AspNetUsers_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatBox_AspNetUsers_FreelancerID",
                        column: x => x.FreelancerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackUser",
                columns: table => new
                {
                    FeedbackUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateByUserID = table.Column<int>(type: "int", nullable: false),
                    TargetToUserID = table.Column<int>(type: "int", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackUser", x => x.FeedbackUserID);
                    table.ForeignKey(
                        name: "FK_FeedbackUser_AspNetUsers_CreateByUserID",
                        column: x => x.CreateByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedbackUser_AspNetUsers_TargetToUserID",
                        column: x => x.TargetToUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryPayment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryPayment", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_HistoryPayment_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    NotificationContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReaded = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportUser",
                columns: table => new
                {
                    ReportUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateByUserID = table.Column<int>(type: "int", nullable: false),
                    TargetToUserID = table.Column<int>(type: "int", nullable: false),
                    ReportUserContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUser", x => x.ReportUserID);
                    table.ForeignKey(
                        name: "FK_ReportUser_AspNetUsers_CreateByUserID",
                        column: x => x.CreateByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportUser_AspNetUsers_TargetToUserID",
                        column: x => x.TargetToUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawRequest",
                columns: table => new
                {
                    WithdrawRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawRequest", x => x.WithdrawRequestID);
                    table.ForeignKey(
                        name: "FK_WithdrawRequest_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmployerID = table.Column<int>(type: "int", nullable: false),
                    FreelancerID = table.Column<int>(type: "int", nullable: true),
                    JobContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobID);
                    table.ForeignKey(
                        name: "FK_Job_AspNetUsers_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_AspNetUsers_FreelancerID",
                        column: x => x.FreelancerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FreelancerWithSkill",
                columns: table => new
                {
                    FreelancerWithSkillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreelancerID = table.Column<int>(type: "int", nullable: false),
                    SkillID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerWithSkill", x => x.FreelancerWithSkillID);
                    table.ForeignKey(
                        name: "FK_FreelancerWithSkill_AspNetUsers_FreelancerID",
                        column: x => x.FreelancerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FreelancerWithSkill_Skill_SkillID",
                        column: x => x.SkillID,
                        principalTable: "Skill",
                        principalColumn: "SkillID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatBoxID = table.Column<int>(type: "int", nullable: false),
                    FromEmployer = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Message_ChatBox_ChatBoxID",
                        column: x => x.ChatBoxID,
                        principalTable: "ChatBox",
                        principalColumn: "ChatBoxID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobInCategory",
                columns: table => new
                {
                    JobInCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    JobID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobInCategory", x => x.JobInCategoryID);
                    table.ForeignKey(
                        name: "FK_JobInCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobInCategory_Job_JobID",
                        column: x => x.JobID,
                        principalTable: "Job",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Milestone",
                columns: table => new
                {
                    MilestoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    MilestoneContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePathFreelancerUpload = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountToPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestone", x => x.MilestoneID);
                    table.ForeignKey(
                        name: "FK_Milestone_Job_JobID",
                        column: x => x.JobID,
                        principalTable: "Job",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreelancerID = table.Column<int>(type: "int", nullable: false),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    DateOffer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProposedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedDays = table.Column<short>(type: "smallint", nullable: false),
                    SelfIntroduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedPlan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.OfferID);
                    table.ForeignKey(
                        name: "FK_Offer_AspNetUsers_FreelancerID",
                        column: x => x.FreelancerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offer_Job_JobID",
                        column: x => x.JobID,
                        principalTable: "Job",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportJob",
                columns: table => new
                {
                    ReportJobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetToJob = table.Column<int>(type: "int", nullable: false),
                    CreateByFreelancer = table.Column<int>(type: "int", nullable: false),
                    ReportJobContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportJob", x => x.ReportJobID);
                    table.ForeignKey(
                        name: "FK_ReportJob_AspNetUsers_TargetToJob",
                        column: x => x.TargetToJob,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportJob_Job_TargetToJob",
                        column: x => x.TargetToJob,
                        principalTable: "Job",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaveJob",
                columns: table => new
                {
                    SaveJobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    FreelancerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveJob", x => x.SaveJobID);
                    table.ForeignKey(
                        name: "FK_SaveJob_AspNetUsers_JobID",
                        column: x => x.JobID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaveJob_Job_JobID",
                        column: x => x.JobID,
                        principalTable: "Job",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "UserID", "AccessFailedCount", "Address", "AmountMoney", "BankAccountInfor", "BankName", "CVFilePath", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FullNameOnIDCard", "IDNumber", "Introduction", "IsVeryfiedIdentity", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "StarMedium", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, null, null, "407f1539-ec29-453a-85ab-d83fbc262438", null, "nva123@gmail.com", true, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Nguyen Van A" },
                    { 2, 2, null, null, null, null, null, "2b9515d0-f428-4486-ba86-d3f875d87b40", null, "nvb123@gmail.com", true, null, null, null, true, false, null, null, null, "test", null, false, null, null, null, false, "Nguyen Van B" },
                    { 3, 0, null, null, null, null, null, "0ece3227-5e59-4dc4-9752-772561de2127", null, "nvc123@gmail.com", false, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Nguyen Van C" },
                    { 4, 3, null, null, null, null, null, "6113aea3-2102-407c-9c5f-f40a720fdb4b", null, "tvd123@gmail.com", true, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Tran Van D" },
                    { 5, 4, null, null, null, null, null, "34c2fa0b-2d94-4a29-be12-34a0417b5848", null, "tte123@gmail.com", true, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Tran Thi E" },
                    { 6, 5, null, null, null, null, null, "de7a66e1-a4dd-4e2c-873a-97c928e4bf92", null, "tvf123@gmail.com", false, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Tran Van F" },
                    { 7, 6, null, null, null, null, null, "288a6c76-a14f-43f6-aaf3-7b78b75c2e90", null, "ttg123@gmail.com", true, null, null, null, true, false, null, null, null, "test", null, false, null, null, null, false, "Tran Thi G" },
                    { 8, 7, null, null, null, null, null, "0c38746b-38cc-46e3-a4ed-1c8e464c61d5", null, "tth123@gmail.com", true, null, null, null, false, false, null, null, null, "test", null, false, null, null, null, false, "Tran Thi H" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryID", "CategoryName", "CategoryParentID" },
                values: new object[,]
                {
                    { 1, "Web dev", null },
                    { 4, "BA", null },
                    { 5, "Dạy học trực tuyến", null }
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "LocationID", "LocationName" },
                values: new object[,]
                {
                    { 1, "HaNoi" },
                    { 2, "DaNang" }
                });

            migrationBuilder.InsertData(
                table: "SearchJobForm",
                columns: new[] { "FormID", "CategoryID", "LocationID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 5, 2 }
                });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillID", "SkillName", "SkillParentID" },
                values: new object[,]
                {
                    { 1, "Web development", null },
                    { 4, "design 2D", null },
                    { 5, "communication", null },
                    { 6, "English", null },
                    { 7, "Teaching", null }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryID", "CategoryName", "CategoryParentID" },
                values: new object[,]
                {
                    { 2, "BE dev", 1 },
                    { 3, "FE dev", 1 }
                });

            migrationBuilder.InsertData(
                table: "FreelancerWithSkill",
                columns: new[] { "FreelancerWithSkillID", "FreelancerID", "SkillID" },
                values: new object[,]
                {
                    { 1, 4, 1 },
                    { 4, 7, 4 },
                    { 5, 8, 5 },
                    { 6, 4, 6 },
                    { 7, 7, 7 },
                    { 8, 5, 1 },
                    { 11, 8, 4 },
                    { 12, 7, 5 }
                });

            migrationBuilder.InsertData(
                table: "Job",
                columns: new[] { "JobID", "BudgetFrom", "BudgetTo", "DateCreated", "EmployerID", "FreelancerID", "JobContent", "JobTitle", "LocationID", "Status" },
                values: new object[,]
                {
                    { 1, 300000m, 500000m, new DateTime(2023, 1, 29, 21, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, "Minh can 1 nguoi code web", "Tim DEV", 1, "Hired" },
                    { 2, 200000m, 800000m, new DateTime(2023, 1, 29, 21, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Can 1 nguoi hieu ve nghiep vu ngan hang để tạo ra tài liệu requirement cho trang web", "Tim BA làm requirement", 1, "Approved" },
                    { 3, 400000m, 1000000m, new DateTime(2023, 1, 29, 22, 0, 0, 0, DateTimeKind.Unspecified), 2, 8, "Can nguoi giup minh thiet ke DataBase cho trang web giao duc", "Thiet ke Database", 2, "Hired" }
                });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillID", "SkillName", "SkillParentID" },
                values: new object[,]
                {
                    { 2, "code React", 1 },
                    { 3, "code .net razor page", 1 }
                });

            migrationBuilder.InsertData(
                table: "FreelancerWithSkill",
                columns: new[] { "FreelancerWithSkillID", "FreelancerID", "SkillID" },
                values: new object[,]
                {
                    { 2, 5, 2 },
                    { 3, 6, 3 },
                    { 9, 6, 2 },
                    { 10, 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "JobInCategory",
                columns: new[] { "JobInCategoryID", "CategoryID", "JobID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 2 },
                    { 5, 1, 3 },
                    { 6, 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "Milestone",
                columns: new[] { "MilestoneID", "AmountToPay", "Deadline", "FilePathFreelancerUpload", "JobID", "MilestoneContent" },
                values: new object[,]
                {
                    { 1, 100000m, new DateTime(2024, 2, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Job1Moc1.pdf", 1, "Moc 1:..." },
                    { 2, 300000m, new DateTime(2024, 2, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), "Job1Moc2.pdf", 1, "Moc 2:..." },
                    { 3, 100000m, new DateTime(2024, 2, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), "Job3Moc1.pdf", 3, "Moc 1:..." },
                    { 4, 200000m, new DateTime(2024, 2, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Job3Moc2.pdf", 3, "Moc 2:..." },
                    { 5, 300000m, new DateTime(2024, 2, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Job3Moc3.pdf", 3, "Moc 3:..." }
                });

            migrationBuilder.InsertData(
                table: "Offer",
                columns: new[] { "OfferID", "DateOffer", "EstimatedPlan", "ExpectedDays", "FreelancerID", "JobID", "ProposedCost", "SelfIntroduction" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "Nếu bạn quan tâm đến chào giá này, hãy reply cho mình biết. Mình sẽ setup một buổi meeting trao đổi chi tiết về doanh nghiệp của bạn và gửi sitemap, kế hoạch chi tiết trong vòng không quá 2 giờ sau đó.", (short)7, 5, 1, 280000m, "Đã có kinh nghiệm 3 năm làm web, mobile app đa lĩnh vực trong và ngoài nước" },
                    { 2, new DateTime(2023, 1, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kế hoạch thực hiện công việc:\r\n-Thu thập thông tin khách hàng cũng nhu nhu cầu thiết kế.\r\n-Phân tích, báo giá và tiến hành thương lượng chốt sản phẩm.\r\n-Hoàn thành sản phẩm trong tiến độ đã thương lượng, test sản phẩm và bàn giao đến khách hàng.\r\n-Tiến hành sửa chữa, fix lỗi trong quá trình dùng thử.\r\n-Nhận thanh toán và áp dụng chính sách hậu mãi cho khách hàng", (short)3, 8, 1, 500000m, "Tôi là một lập trình viên có nhiều năm kinh nghiệm phát triển các loại website, đặc biệt là các trang web bán hàng, giáo dục, bất động sản và y tế. Trong suốt sự nghiệp của mình, tôi tạo ra những trang web chất lượng cao, mang tính sáng tạo và tối ưu hoá hiệu suất. Tôi tự hào về việc đã đóng góp vào việc xây dựng nền tảng kỹ thuật vững chắc để hỗ trợ các doanh nghiệp bán hàng và các tổ chức giáo dục trong việc tăng cường hiệu quả kinh doanh và phục vụ học tập.\r\n\r\nTrong quá trình làm việc, tôi đã tiếp xúc và thành thạo các công nghệ đa dạng như HTML, CSS, JavaScript, PHP, Python và nhiều framework phổ biến như Vuejs, Nuxtjs, Reactjs, Nextjs, Laravel, Django, Express, Nestjs. Sự am hiểu sâu sắc về các công nghệ này giúp tôi tạo ra những trải nghiệm người dùng tuyệt vời và tích hợp những tính năng đa dạng, như thanh toán an toàn, quản lý tài khoản, đánh giá sản phẩm và nhiều tính năng tùy chỉnh khác." },
                    { 3, new DateTime(2023, 1, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "Meeting trao doi chi tiet ve Requirement, sau do lam Database", (short)7, 4, 3, 1000000m, "Toi co 2 nam kinh nghiem lam DataBase" },
                    { 4, new DateTime(2023, 2, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), "Làm ngay sau khi có đầy đủ nội dung và yêu cầu. Bảo hành và bảo trì.", (short)7, 5, 2, 700000m, "" },
                    { 5, new DateTime(2023, 2, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), "Thu thập thông tin về requirement, phân tích, báo giá & thương lượng, hoàn thành theo tiến độ đã vạch ra, test, hỗ trợ 1 tháng sau khi bàn giao", (short)4, 8, 3, 800000m, "Toi co 3 nam kinh nghiem lam DataBase cho cong ty cong nghe noi tieng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryParentID",
                table: "Category",
                column: "CategoryParentID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBox_EmployerID",
                table: "ChatBox",
                column: "EmployerID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBox_FreelancerID",
                table: "ChatBox",
                column: "FreelancerID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackUser_CreateByUserID",
                table: "FeedbackUser",
                column: "CreateByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackUser_TargetToUserID",
                table: "FeedbackUser",
                column: "TargetToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerWithSkill_FreelancerID",
                table: "FreelancerWithSkill",
                column: "FreelancerID");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerWithSkill_SkillID",
                table: "FreelancerWithSkill",
                column: "SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryPayment_UserID",
                table: "HistoryPayment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Job_EmployerID",
                table: "Job",
                column: "EmployerID");

            migrationBuilder.CreateIndex(
                name: "IX_Job_FreelancerID",
                table: "Job",
                column: "FreelancerID");

            migrationBuilder.CreateIndex(
                name: "IX_Job_LocationID",
                table: "Job",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_JobInCategory_CategoryID",
                table: "JobInCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_JobInCategory_JobID",
                table: "JobInCategory",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatBoxID",
                table: "Message",
                column: "ChatBoxID");

            migrationBuilder.CreateIndex(
                name: "IX_Milestone_JobID",
                table: "Milestone",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_FreelancerID",
                table: "Offer",
                column: "FreelancerID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_JobID",
                table: "Offer",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportJob_TargetToJob",
                table: "ReportJob",
                column: "TargetToJob");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUser_CreateByUserID",
                table: "ReportUser",
                column: "CreateByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUser_TargetToUserID",
                table: "ReportUser",
                column: "TargetToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SaveJob_JobID",
                table: "SaveJob",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_SkillParentID",
                table: "Skill",
                column: "SkillParentID");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequest_UserID",
                table: "WithdrawRequest",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlockWordRegEx");

            migrationBuilder.DropTable(
                name: "FeedbackUser");

            migrationBuilder.DropTable(
                name: "FreelancerWithSkill");

            migrationBuilder.DropTable(
                name: "HistoryPayment");

            migrationBuilder.DropTable(
                name: "JobInCategory");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Milestone");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "ReportJob");

            migrationBuilder.DropTable(
                name: "ReportUser");

            migrationBuilder.DropTable(
                name: "SaveJob");

            migrationBuilder.DropTable(
                name: "SearchJobForm");

            migrationBuilder.DropTable(
                name: "WithdrawRequest");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ChatBox");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
