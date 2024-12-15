using FYP.Db;
using FYP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Databases
{
    public static class DbSeeder
    {
        // Method to seed roles and create admin user
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Retrieve UserManager, RoleManager, and DbContext from the service provider
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed Roles if they don't exist
            await SeedRolesAsync(roleManager);

            // Create admin user if not already created
        }

        // Helper method to seed roles if they don't exist
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        private static async Task CreateAdminUserAsync(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            var adminEmail = "FypAdmin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Faraz",
                    ActiveState = "true",
                    Role = Roles.Admin.ToString(),
                    EmailConfirmed = true
                };

                // Create the user
                var result = await userManager.CreateAsync(user, "Admin@@123");
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}
