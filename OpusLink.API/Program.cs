using AutoMapper;
using OpusLink.Entity;
using OpusLink.Entity.AutoMapper;
using OpusLink.Service.JobServices;
using Microsoft.Extensions.DependencyInjection;
using OpusLink.Entity.AutoMapper.JOB;

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
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperConfig());
            mc.AddProfile(new JobProfile());
            mc.AddProfile(new CategoryProfile());
            mc.AddProfile(new OfferProfile());
            mc.AddProfile(new SaveJobProfile());
            mc.AddProfile(new LocationProfile());
        });
        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddScoped<IJobService, JobService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOfferService, OfferService>();
        builder.Services.AddScoped<ISaveJobService, SaveJobService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IJobAndCategoryService, JobAndCategoryService>();

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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}