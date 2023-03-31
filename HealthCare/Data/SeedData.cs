using System;
using System.Linq;
using HealthCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare.Data
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Users.Any(x=>x.UserName == "admin@gmail.com"))
            {
                return; // Database has been seeded
            }

            // Create a default admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Role="Admin",
                IsCoach= false,
            };

            var password = "Admin@123";
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var result = await userManager.CreateAsync(adminUser, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}