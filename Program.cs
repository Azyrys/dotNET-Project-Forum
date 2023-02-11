using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using dotNET_Project.Data;
using dotNET_Project.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("dotNET_ProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'dotNET_ProjectContextConnection' not found.");

builder.Services.AddDbContext<dotNET_ProjectContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<dotNET_ProjectUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<dotNET_ProjectContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
