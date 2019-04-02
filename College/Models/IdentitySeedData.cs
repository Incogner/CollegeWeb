using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        private const string genUser = "General";
        private const string genPassword = "Password123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
                .GetRequiredService<AppIdentityDbContext>();
            context.Database.Migrate();
            if (!context.Roles.Any())//populate roles if there is no one in context
            {
                Func<Task> func = async () =>
                {
                    RoleManager<IdentityRole> roleManager = app.ApplicationServices
                        .GetRequiredService<RoleManager<IdentityRole>>();
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        var role = new IdentityRole("Admin");
                        await roleManager.CreateAsync(role);
                    }
                    if (!await roleManager.RoleExistsAsync("Student"))
                    {
                        var role = new IdentityRole("Student");
                        await roleManager.CreateAsync(role);
                    }
                };
                Task task = func();
                task.Wait();
            }


            if (!context.Users.Any())//populate users if there is no one in context
            {
                UserManager<IdentityUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();
                IdentityUser user = await userManager.FindByIdAsync(adminUser);
                if (user == null)
                {
                    user = new IdentityUser("Admin");
                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                IdentityUser user2 = await userManager.FindByIdAsync(genUser);
                if (user2 == null)
                {
                    user2 = new IdentityUser("General");
                    await userManager.CreateAsync(user2, genPassword);
                    await userManager.AddToRoleAsync(user2, "Student");
                }
                IdentityUser user3 = await userManager.FindByIdAsync("Student1");
                if (user3 == null)
                {
                    user3 = new IdentityUser("Student1");
                    await userManager.CreateAsync(user3, "Password123$");
                    await userManager.AddToRoleAsync(user3, "Student");
                }
                IdentityUser user4 = await userManager.FindByIdAsync("Student2");
                if (user4 == null)
                {
                    user4 = new IdentityUser("Student2");
                    await userManager.CreateAsync(user4, "Password123$");
                    await userManager.AddToRoleAsync(user4, "Student");
                }
                IdentityUser user5 = await userManager.FindByIdAsync("Student3");
                if (user5 == null)
                {
                    user5 = new IdentityUser("Student3");
                    await userManager.CreateAsync(user5, "Password123$");
                    await userManager.AddToRoleAsync(user5, "Student");
                }
            }
        }
    }
}
