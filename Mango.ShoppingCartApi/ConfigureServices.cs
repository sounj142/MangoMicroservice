using Azure.Messaging.ServiceBus;
using Commons.Services;
using Mango.ShoppingCartApi.Mappers;
using Mango.ShoppingCartApi.Repositories;
using Mango.ShoppingCartApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Mango.ShoppingCartApi;

public class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        services.AddControllers();

        services.AddHttpClient();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        var scheme = JwtBearerDefaults.AuthenticationScheme;
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.AddSecurityDefinition(scheme,
                new OpenApiSecurityScheme
                {
                    Description = "Enter 'Beader {token}' to call API",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = scheme
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = scheme
                       },
                       Scheme = "oauth2",
                       Name = scheme,
                       In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        services.AddAuthentication(scheme)
            .AddJwtBearer(scheme, options =>
            {
                options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "MangoApp");
            });
        });

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddHttpContextAccessor();
        ConfigureAzureServiceBus(builder);

        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IProductService, ProductService>();
    }

    private static void ConfigureAzureServiceBus(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton(provider => new ServiceBusClient(
            builder.Configuration["AzureServiceBus:ConnectionString"]));

        services.AddSingleton(provider => new CheckoutQueueSender(
            provider.GetRequiredService<ServiceBusClient>(),
            builder.Configuration["AzureServiceBus:CheckoutQueue"]));

        services.AddSingleton(provider => new ProductSavedServiceBusReceiver(
            provider,
            builder.Configuration["AzureServiceBus:ProductSavedTopic"],
            builder.Configuration["AzureServiceBus:ShoppingCartSubscriptionName"]));
    }
}