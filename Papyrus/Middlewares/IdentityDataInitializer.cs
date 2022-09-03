using Microsoft.AspNetCore.Identity;
using Papyrus.DataAccess.Entities;
using Papyrus.DataAccess.Enums;

namespace Papyrus.Middlewares;

public static class IdentityDataInitializer
{
    private static readonly List<ServerRole> ROLES = new() { ServerRole.Administrator, ServerRole.Moderator, ServerRole.User };

    public static void SeedRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            ROLES.ForEach(roleName =>
            {
                var exists = roleManager.RoleExistsAsync(roleName.ToString()).Result;
                if (!exists)
                {
                    var role = new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = roleName.ToString()
                    };
                    var result = roleManager.CreateAsync(role).Result;
                }
            });
        }
        catch (Exception e)
        {
            // ignored
            Console.WriteLine(e.Message);
        }
    }
}
