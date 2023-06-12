using Microsoft.AspNetCore.Identity;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Extensions
{
    static class PredefinedUsers
    {
        public static void CreateAdminUser(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roleName = "Administrator";
                IdentityResult result;

                var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    result = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
                    if (result.Succeeded)
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                        var admin = userManager.FindByEmailAsync(config["AdminCredentials:Email"]).Result;

                        if (admin == null)
                        {
                            admin = new IdentityUser()
                            {
                                UserName = config["AdminCredentials:Email"],
                                Email = config["AdminCredentials:Email"],
                                EmailConfirmed = true
                            };

                            result = userManager.CreateAsync(admin, config["AdminCredentials:Password"]).Result;
                            if (result.Succeeded)
                            {
                                result = userManager.AddToRoleAsync(admin, roleName).Result;
                                if (!result.Succeeded)
                                {
                                    throw new Exception("Error adding administrator");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
