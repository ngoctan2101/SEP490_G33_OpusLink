using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OpusLinkDBContextConnection") ?? throw new InvalidOperationException("Connection string 'OpusLinkDBContextConnection' not found.");

builder.Services.AddDbContext<OpusLinkDBContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();

app.MapRazorPages();

app.Run();
