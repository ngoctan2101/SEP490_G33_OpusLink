using AutoMapper;
using OpusLink.Entity;
using OpusLink.Entity.AutoMapper;
using OpusLink.Service.JobServices;
using OpusLink.Service.Admin;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpusLink.Entity.AutoMapper.JOB;
using OpusLink.Service.Chat;
using OpusLink.Service.AccountServices;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO.SendEmail;
using OpusLink.Service.UserServices;
using Microsoft.OpenApi.Models;
using OpusLink.Service.PaymentServices;
using OpusLink.Service.ValidationServices;
using OpusLink.Service.WithDrawRequestServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Authorization
        /*builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OpusLink API",
                Version = "v1"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });*/
        builder.Services.AddAuthentication(); // Sử dụng dịch vụ Authentication
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorPolicy", build => build.AllowAnyMethod()
            .AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(hostName => true).Build());
        });
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperConfig());
            mc.AddProfile(new JobProfile());
            mc.AddProfile(new CategoryProfile());
            mc.AddProfile(new SkillMapper());
            mc.AddProfile(new UserMapper());
            mc.AddProfile(new OfferProfile());
            mc.AddProfile(new SaveJobProfile());
            mc.AddProfile(new LocationProfile());
            mc.AddProfile(new UserProfile());
            mc.AddProfile(new ChatMapper());
            mc.AddProfile(new HistoryPaymentMapper());
            mc.AddProfile(new WithdrawRequesMapper());

        });
        IMapper mapper = mapperConfig.CreateMapper();

        InjectDependencyServices(builder.Services, builder.Configuration);
        ConfigureJwt(builder.Services, builder.Configuration);

        builder.Services.AddScoped<IJobService, JobService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ISkillService, SkillService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IOfferService, OfferService>();
        builder.Services.AddScoped<ISaveJobService, SaveJobService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IJobAndCategoryService, JobAndCategoryService>();
        builder.Services.AddScoped<IFreelancerAndSkillService, FreelancerAndSkillService>();
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IHistoryPaymentService, HistoryPaymentService>();
        builder.Services.AddScoped<IValidationService, ValidationService>();
        builder.Services.AddScoped<IWithDrawRequestService, WithDrawRequestService>();

        builder.Services.AddDbContext<OpusLinkDBContext>();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddCors();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
/*        app.UseAuthorization();*/

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }

    private static void InjectDependencyServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OpusLinkDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OpusLink"));
        });

        //Map cái interface và class với nhau
        services.AddTransient<IAccountService, AccountService>();
    }

    //Config JWT
    private static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTSetting>(configuration.GetSection("JwtSetting"));
        services.AddDbContext<OpusLinkDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OpusLink"));
        });
        services.AddIdentity<User, Role>().AddEntityFrameworkStores<OpusLinkDBContext>().AddDefaultTokenProviders();
        services.AddIdentityCore<User>();
        services.Configure<IdentityOptions>(options =>
        {
            //setting for password
            options.Password.RequireDigit = false; //Bắt buộc phải có số không
            options.Password.RequireLowercase = false; //Bắt buộc phải có chữ thường không
            options.Password.RequireUppercase = false; //Bắt buộc phải có chữ hoa không
            options.Password.RequireNonAlphanumeric = false; //
            options.Password.RequiredLength = 6; //Max length = 6

            //setting for logout
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); //Trong 30 phút
            options.Lockout.MaxFailedAccessAttempts = 3; //Login quá 3 lần -> Khóa lại
            options.Lockout.AllowedForNewUsers = true;

            //setting for user
            options.User.RequireUniqueEmail = true; //Không được đăng kí trùng email

            //setting for signin
            options.SignIn.RequireConfirmedEmail = false;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            //Validate token sẽ bao gồm những gì? - Validate (check xem thông tin có giống không)
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSetting:Issuer"],
                ValidAudience = configuration["JwtSetting:Issuer"],

                //Đây là key bảo mật riêng Signature -> Mã hóa sang 1 mã SecurityKey
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:Key"]))
            };
        });

        // Add Email Configs
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);

        /*services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(30);
        });*/
    }
}