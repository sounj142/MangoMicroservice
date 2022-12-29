using Mango.Web.Mappers;
using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Mango.Web;

public static class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllersWithViews();
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICartService, CartService>();

        services.AddSingleton((provider) =>
            builder.Configuration.GetSection("ServiceUrls").Get<ServiceUrls>());

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies", cookies =>
        {
            cookies.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ClientId = "MangoWeb";
            options.ClientSecret = builder.Configuration["Identity:Secret"];
            options.ResponseType = "code";
            options.ClaimActions.MapJsonKey("role", "role", "role");
            options.ClaimActions.MapJsonKey("sub", "sub", "sub");

            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.RoleClaimType = "role";
            options.Scope.Add("MangoApp");
            options.SaveTokens = true;
        });
    }
}