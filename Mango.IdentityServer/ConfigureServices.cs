using Mango.IdentityServer.Models;
using Mango.IdentityServer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.IdentityServer;

public static class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllersWithViews();

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var identityServerBuilder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.EmitStaticAudienceClaim = true;
        })
        .AddInMemoryIdentityResources(Statics.IdentityResources)
        .AddInMemoryApiScopes(Statics.ApiScopes)
        .AddInMemoryClients(Statics.GenerateClients(builder.Configuration))
        .AddAspNetIdentity<ApplicationUser>();

        if (builder.Environment.IsDevelopment())
        {
            identityServerBuilder.AddDeveloperSigningCredential();
        }
    }
}