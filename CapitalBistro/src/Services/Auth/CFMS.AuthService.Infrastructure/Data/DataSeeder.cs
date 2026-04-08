using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CFMS.AuthService.Core.Entities;
using CFMS.AuthService.Core.Interfaces;

namespace CFMS.AuthService.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AuthDbContext>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

            // Auto-apply migrations
            await context.Database.MigrateAsync();

            var roles = new[] { "Admin", "Manager", "Staff" };
            
            foreach (var roleName in roles)
            {
                if (!await context.Roles.AnyAsync(r => r.Name == roleName))
                {
                    await context.Roles.AddAsync(new Role { Name = roleName, Description = $"System {roleName} Role" });
                }
            }
            await context.SaveChangesAsync();

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            
            if (adminRole != null && !await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = passwordHasher.HashPassword("Admin@123"), // Default password for admin
                    RoleId = adminRole.Id,
                    FranchiseId = null // Null means HQ Administrator
                };
                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
