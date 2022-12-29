using IdentityModel;
using Mango.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mango.IdentityServer.Repositories;

public class DatabaseMigration
{
    public static async Task InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        try
        {
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();
            await SeedInitialRoles(roleManager);
            await SeedInitialUsers(userManager);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseMigration>>();
            logger.LogError(ex, "An error occured during migrate database");
            throw;
        }
    }

    private static async Task SeedInitialRoles(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = Statics.AdminRole,
            },
            new IdentityRole
            {
                Name = Statics.CustomerRole,
            }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    private static async Task SeedInitialUsers(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        const string password = "111111";
        var adminUser = new ApplicationUser
        {
            UserName = "tazan645",
            Email = "tazan645@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111",
            FirstName = "David",
            LastName = "Mayer"
        };
        await userManager.CreateAsync(adminUser, password);
        await userManager.AddToRoleAsync(adminUser, Statics.AdminRole);
        await userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            //new Claim(JwtClaimTypes.Name, adminUser.UserName),
            //new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
            //new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
            //new Claim(JwtClaimTypes.Role, Statics.AdminRole),
            new Claim(JwtClaimTypes.WebSite, "http://"+adminUser.UserName+".com"),
        });

        var customerUser = new ApplicationUser
        {
            UserName = "sounj",
            Email = "sounj142@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "2222222222",
            FirstName = "Le",
            LastName = "Huy"
        };
        await userManager.CreateAsync(customerUser, password);
        await userManager.AddToRoleAsync(customerUser, Statics.CustomerRole);
        await userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            //new Claim(JwtClaimTypes.Name, customerUser.UserName),
            //new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
            //new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
            //new Claim(JwtClaimTypes.Role, Statics.CustomerRole),
            new Claim(JwtClaimTypes.WebSite, "http://"+customerUser.UserName+".com"),
        });
    }
}