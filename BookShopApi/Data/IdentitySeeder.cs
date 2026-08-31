using BookShopApi.Constants;
using BookShopApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            foreach (var roleName in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var users = await userManager.Users.OrderBy(user => user.CreatedAt).ToListAsync();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                    await userManager.AddToRoleAsync(user, AppRoles.User);
            }

            var managers = await userManager.GetUsersInRoleAsync(AppRoles.Manager);
            if (managers.Count == 0)
            {
                var firstUser = users.FirstOrDefault();
                if (firstUser != null)
                {
                    var currentRoles = await userManager.GetRolesAsync(firstUser);
                    if (currentRoles.Count > 0)
                        await userManager.RemoveFromRolesAsync(firstUser, currentRoles);

                    await userManager.AddToRoleAsync(firstUser, AppRoles.Manager);
                }
            }
        }
    }
}
