using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;

namespace Mango.Gateway;

public class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        var scheme = JwtBearerDefaults.AuthenticationScheme;
        services.AddAuthentication(scheme)
            .AddJwtBearer(scheme, options =>
            {
                options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        services.AddOcelot();

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("ApiScope", policy =>
        //    {
        //        policy.RequireAuthenticatedUser();
        //        policy.RequireClaim("scope", "MangoApp");
        //    });
        //});

        //services.AddHttpContextAccessor();
        //services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        //services.AddScoped<ICartService, CartService>();
    }
}