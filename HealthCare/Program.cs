using HealthCare.Data;
using HealthCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Athlete", policy =>
        policy.RequireRole("Athlete"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


// Create default roles for Coach and Athlete
var serviceProvider = builder.Services.BuildServiceProvider();
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}

if (!await roleManager.RoleExistsAsync("Athlete"))
{
    await roleManager.CreateAsync(new IdentityRole("Athlete"));
}

if (!await roleManager.RoleExistsAsync("Coach"))
{
    await roleManager.CreateAsync(new IdentityRole("Coach"));
}

var app = builder.Build();
SeedData.Initialize(serviceProvider);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();