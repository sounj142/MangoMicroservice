using Mango.Web.Mappers;
using Mango.Web.Models;
using Mango.Web.Services;

namespace Mango.Web;

public static class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllersWithViews();
        services.AddHttpClient();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddSingleton((provider) =>
            builder.Configuration.GetSection("ServiceUrls").Get<ServiceUrls>());

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
    }
}